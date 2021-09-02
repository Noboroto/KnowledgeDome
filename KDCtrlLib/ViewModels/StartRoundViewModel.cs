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
using System.Threading;

namespace KDCtrlLib.ViewModels
{
	public class StartRoundViewModel : ViewModelBase
	{
		private const int TIME_LIMIT = 60;
		private const double TIME_AMOUNT = 0.01;

		private CancellationTokenSource cancellation = new CancellationTokenSource();
		private Player _CurrentPlayer;
		private StartQuestion _CurrentQuestion;
		private int _QuestCount;
		private double _Timer;
		private bool _Running;
		private bool _TimerEnable;
		private bool _DoneEnable;
		private bool _SoundEnable;

		public bool TimerEnable
		{
			get => _TimerEnable;
			set => Set(ref _TimerEnable, value);
		}
		public bool DoneEnable
		{
			get => _DoneEnable;
			set => Set(ref _DoneEnable, value);
		}
		public bool Running
		{
			get => _Running;
			set => Set(ref _Running, value);
		}
		public bool SoundEnable
		{
			get => _SoundEnable;
			set => Set(ref _SoundEnable, value);
		}
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
		public ICommand SoundCmd { get; set; }
		public ICommand StopCmd { get; set; }
		public ICommand DoneCmd { get; set; }

		public StartRoundViewModel()
		{
			#region DEBUG_DATA
			Data.InitializeForDevelop();
			#endregion

			#region INIT
			DoneEnable = false;
			SoundEnable = false;
			TimerEnable = true;
			Running = false;
			Timer = TIME_LIMIT;
			CurrentPlayer = Data.CurrentMatch.Players[Data.CurrentPlayerIndex];
			#endregion

			#region CLIENT INIT
			if (Data.ThisMacineType != MachineType.Server)
			{
				StartCommandChecker();
			}
			#endregion

			#region Cmd
			StartTimmerCmd = new RelayCommand(async () =>
			{
				try
				{
					TimerEnable = false;
					Running = true;
					await TimerStart();
				}
				catch (OperationCanceledException)
				{
					Timer = 0;
				}
				finally
				{
					cancellation.Dispose();
				}
			});
			RightCmd = new RelayCommand(() => RightAns());
			WrongCmd = new RelayCommand(() => WrongAns());
			SoundCmd = new RelayCommand(() => { return; });
			StopCmd = new RelayCommand(() => 
			{
				var res = MessageBox.Show("Bạn có muốn dừng khẩn cấp không?", "Dừng khẩn cấp", MessageBoxButton.YesNo, MessageBoxImage.Stop);
				if (res == MessageBoxResult.No)
					return;
				if (!cancellation.IsCancellationRequested) cancellation.Cancel();
				Running = false;
				DoneEnable = true;
			});
			DoneCmd = new RelayCommand(()=> { return; });
			#endregion
		}
		private Task TimerStart()
		{
			return Task.Run(async () =>
			{
				try
				{
					CurrentQuestion = GetNewQuestion(Data.CurrentMatch.StartQuestions);
					SoundEnable = CurrentQuestion.AttachmentInfo == AttachmentType.Sound;
				}
				catch (ArgumentOutOfRangeException)
				{
					MessageBox.Show("Đã hết câu hỏi");
					return;
				}
				while (Timer > 0)
				{
					Timer -= TIME_AMOUNT;
					cancellation.Token.ThrowIfCancellationRequested();
					await Task.Delay(System.TimeSpan.FromSeconds(TIME_AMOUNT));
				}
				Running = false;
				DoneEnable = true;
			}, cancellation.Token
			);
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
			SoundEnable = CurrentQuestion.AttachmentInfo == AttachmentType.Sound;
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
			SoundEnable = CurrentQuestion.AttachmentInfo == AttachmentType.Sound;
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
