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
using GalaSoft.MvvmLight.Messaging;
using KDLib.MessageForUI;

namespace KDCtrlLib.ViewModels
{
	public class StartRoundViewModel : ViewModelBase
	{
		private const int TIME_LIMIT = 60;
		private const double TIME_AMOUNT = 0.01;

		private CancellationTokenSource cancellation = new CancellationTokenSource();
		private StartQuestion _CurrentQuestion;
		private int _QuestCount;
		private double _Timer;
		private bool _TimerEnable;
		private bool _DoneEnable;
		private bool _SoundEnable;
		private int _StoreItemCounter;

		public int StoreItemCounter
		{
			get => _StoreItemCounter;
			set => Set(ref _StoreItemCounter, value);
		}
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
		public bool SoundEnable
		{
			get => _SoundEnable;
			set => Set(ref _SoundEnable, value);
		}
		public Player CurrentPlayer
		{
			get => Data.CurrentPlayer;
			set
			{
				Data.CurrentPlayer = value;
				RaisePropertyChanged(nameof(CurrentPlayer));
			}
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
			#region INIT
			DoneEnable = false;
			SoundEnable = false;
			TimerEnable = true;
			Timer = TIME_LIMIT;
			#endregion

			#region CLIENT INIT
			if (Data.ThisMacineType != Machine.Server)
			{
				StartCommandChecker(cancellation.Token);
			}
			#endregion

			#region Cmd
			StartTimmerCmd = new RelayCommand(async () =>
			{
				CurrentQuestion = GetNewQuestion(Data.StartQuestions);
				NetServer.SendCommandToAll(new KDCommand(CommandType.StartTimmer));
				try
				{
					TimerEnable = false;
					await TimerStart(cancellation.Token);
				}
				catch (OperationCanceledException)
				{
					Timer = 0;
				}
				catch (AggregateException)
				{
					cancellation.Cancel();
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
				DoneEnable = true;
				Messenger.Default.Send(new ChangeState(ProgramState.Pending));
			});
			DoneCmd = new RelayCommand(() => { return; });
			#endregion
		}
		private Task TimerStart(CancellationToken token)
		{
			return Task.Run(async () =>
			{
				Messenger.Default.Send(new ChangeState(ProgramState.Playing));
				SoundEnable = CurrentQuestion.AttachmentInfo == AttachmentType.Sound;
				while (Timer > 0)
				{
					Timer -= TIME_AMOUNT;
					if (token.IsCancellationRequested) break;
					await Task.Delay(System.TimeSpan.FromSeconds(TIME_AMOUNT));
				}
				Messenger.Default.Send(new ChangeState(ProgramState.Pending));
				DoneEnable = true;
			}, token
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
			StoreItemCounter = source.Count;
			return tmp;
		}

		private void OutOfQuestion()
		{
			MessageBox.Show("Đã hết câu hỏi");
			Messenger.Default.Send(new ChangeState(ProgramState.Pending));
			DoneEnable = true;
			cancellation.Cancel();
		}

		private void RightAns()
		{
			if (Data.StartQuestions.Count <= 0)
			{
				OutOfQuestion();
				return;
			}
			CurrentPlayer.Score += 10;
			QuestCount++;
			CurrentQuestion = GetNewQuestion(Data.StartQuestions);
			SoundEnable = CurrentQuestion.AttachmentInfo == AttachmentType.Sound;
			NetServer.SendCommandToAll(new KDCommand(CommandType.Right));
		}

		private void WrongAns()
		{
			if (Data.StartQuestions.Count <= 0)
			{
				OutOfQuestion();
				return;
			}
			QuestCount++;
			CurrentQuestion = GetNewQuestion(Data.StartQuestions);
			SoundEnable = CurrentQuestion.AttachmentInfo == AttachmentType.Sound;
			NetServer.SendCommandToAll(new KDCommand(CommandType.Wrong));
		}

		private void StartCommandChecker(CancellationToken token)
		{
			Task.Run(() =>
			{
				while (true)
				{
					if (token.IsCancellationRequested) return;
					while (Data.Commands.Count > 0)
					{
						try
						{
							KDCommand command = Data.Commands.Peek();
							switch (command.PrefixCmd)
							{
								case CommandType.StartTimmer:
									Task.Run(async() =>
										{
											try
											{
												TimerEnable = false;
												await TimerStart(cancellation.Token);
											}
											catch (OperationCanceledException)
											{
												Timer = 0;
											}
											catch (AggregateException)
											{
												cancellation.Cancel();
											}
										});
									goto EndCommand;
								case CommandType.NextQuestAt:
									CurrentQuestion = Data.StartQuestions[int.Parse(command.Content)];
									goto EndCommand;
								case CommandType.Right:
									CurrentPlayer.Score+=10;
									QuestCount++;
									goto EndCommand;
								case CommandType.Wrong:
									QuestCount++;
									goto EndCommand;
								case CommandType.StopEmergency:
									if (!cancellation.IsCancellationRequested) cancellation.Cancel();
									DoneEnable = true;
									goto EndCommand;
								EndCommand:
									if (Data.Commands.Count > 0) Data.Commands.Dequeue();
									continue;
								default:
									break;
							}
						}
						catch (NullReferenceException)
						{
							continue;
						}
						catch (InvalidOperationException)
						{
							continue;
						}
					}
				}
			}, cancellation.Token);
		}
	}
}
