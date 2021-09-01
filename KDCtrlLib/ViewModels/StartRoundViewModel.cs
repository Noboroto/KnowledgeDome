using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

using KDLib;
using KDCtrlLib.Interface;

using System.Threading.Tasks;
using System.Windows.Media;
using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.Windows;

namespace KDCtrlLib.ViewModels
{
	public class StartRoundViewModel : ViewModelBase
	{
		private const int TIME_LIMIT = 60;

		private Player _CurrentPlayer;
		private StartQuestion _CurrentQuestion;
		private int _QuestCount;
		private double _Timer;

		public Player CurrentPlayer
		{
			get => _CurrentPlayer;
			set => Set(ref _CurrentPlayer, value);
		}
		public StartQuestion CurrentQuestion
		{
			get => _CurrentQuestion;
			set => Set(ref _CurrentQuestion, value);
		}
		public int QuestCount
		{
			get => _QuestCount;
			set => Set(ref _QuestCount, value);
		}
		public double Timer
		{
			get => _Timer;
			set => Set(ref _Timer, value);
		}

		public ICommand StartTimmerCmd { get; set; }
		public ICommand RightCmd { get; set; }
		public ICommand WrongCmd { get; set; }

		public StartRoundViewModel()
		{
			#region DEBUG_DATA
			Data.InitializeForDevelop();
			#endregion

			#region INIT
			Timer = TIME_LIMIT;
			CurrentPlayer = Data.CurrentMatch.Players[Data.CurrentPlayerIndex];
			#endregion

			#region SERVER INIT
			if (Data.ThisMacineType == MachineType.Server)
			{
				try
				{
					CurrentQuestion = GetNewQuestion(Data.CurrentMatch.StartQuestions);
				}
				catch (ArgumentOutOfRangeException)
				{
					MessageBox.Show("Đã hết câu hỏi");
				}
			}
			#endregion

			#region CLIENT INIT
			if (Data.ThisMacineType != MachineType.Server)
			{
				StartCommandChecker();
			}
			#endregion

			#region Cmd
			StartTimmerCmd = new RelayCommand(async () => await TimerStart());
			RightCmd = new RelayCommand(() => RightAns());
			WrongCmd = new RelayCommand(() => WrongAns());
			#endregion
		}


		/// <summary>
		/// Get new StartQuestion from source
		/// </summary>
		/// <returns>
		/// A random StartQuestion
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		private StartQuestion GetNewQuestion(StartQuestionList source)
		{
			var Generator = new Random();
			int index = Generator.Next(source.Count);
			var tmp = source[index];
			source.RemoveAt(index);
			NetServer.SendCommandToAll(new KDCommand(CommandType.NextQuestAt, index.ToString()));
			return tmp;
		}

		private Task TimerStart()
		{
			return Task.Run(async () =>
			{
				while (Timer > 0)
				{
					Timer -= 0.01;
					await Task.Delay(System.TimeSpan.FromSeconds(0.01));
				}
			}
			);
		}

		private void RightAns()
		{
			if (Data.CurrentMatch.StartQuestions.Count <= 0)
			{
				MessageBox.Show("Đã hết câu hỏi");
				return;
			}
			CurrentPlayer.Score += 10;
			QuestCount++;
			CurrentQuestion = GetNewQuestion(Data.CurrentMatch.StartQuestions);
			NetServer.SendCommandToAll(new KDCommand(CommandType.Right));
		}

		private void WrongAns()
		{
			if (Data.CurrentMatch.StartQuestions.Count <= 0)
			{
				MessageBox.Show("Đã hết câu hỏi");
				return;
			}
			QuestCount++;
			CurrentQuestion = GetNewQuestion(Data.CurrentMatch.StartQuestions);
			NetServer.SendCommandToAll(new KDCommand(CommandType.Wrong));
		}

		private Task StartCommandChecker()
		{
			Action ThisAction = () =>
			{
				while (true)
				{
					while (Data.Commands.Count > 0)
					{
						KDCommand command = Data.Commands.Dequeue();
						switch (command.PrefixCmd)
						{
							case CommandType.NextQuestAt:
								CurrentQuestion = Data.CurrentMatch.StartQuestions[int.Parse(command.Content)];
								break;
							case CommandType.Right:
								CurrentPlayer.Score++;
								QuestCount++;
								break;
							case CommandType.Wrong:
								QuestCount++;
								break;
							default:
								break;
						}
					}
				}
			};
			return Task.Run(ThisAction);
		}
	}
}
