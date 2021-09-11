using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

using KDLib;
using KDLib.MessageForUI;

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace KDCtrlLib.ViewModels
{
	public class StartRoundViewModel : ViewModelBase
	{
		private const double TIME_LIMIT = 60;

		private DateTime BeginingTime;
		private DispatcherTimer Timer;
		private CancellationTokenSource cancellation = new CancellationTokenSource();
		private StartQuestion _CurrentQuestion;
		private int _QuestCount;
		private double _Timer;
		private bool _TimerEnable;
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
		public bool SoundEnable => (CurrentQuestion != null) ? CurrentQuestion.AttachmentInfo == AttachmentType.Sound : false;
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
			set
			{
				Set(ref _CurrentQuestion, value);
				RaisePropertyChanged(nameof(SoundEnable));
			}
		}
		public int QuestCount
		{
			get => _QuestCount;
			set => Set(ref _QuestCount, value);
		}
		public double TimerLabel
		{
			get => _Timer;
			set => Set(ref _Timer, value);
		}

		public ICommand StartTimerCmd { get; set; }
		public ICommand RightCmd { get; set; }
		public ICommand WrongCmd { get; set; }
		public ICommand SoundCmd { get; set; }
		public ICommand StopCmd { get; set; }
		public ICommand DoneCmd { get; set; }

		public StartRoundViewModel()
		{
			#region INIT
			TimerEnable = true;
			TimerLabel = TIME_LIMIT;
			Timer = new DispatcherTimer();
			Timer.Tick += new EventHandler(TimerChange);
			Timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
			#endregion

			#region CLIENT INIT
			if (Data.ThisMacineType != Machine.Server)
			{
				Data.RoundCommnads = new KDCommandList(CommandChecker);
			}
			#endregion

			#region Cmd
			StartTimerCmd = new RelayCommand(() => TimerStart());
			RightCmd = new RelayCommand(() => RightAns());
			WrongCmd = new RelayCommand(() => WrongAns());
			SoundCmd = new RelayCommand(() =>
			{

				Messenger.Default.Send(new LogMess(KDLogger.Info($"Chạy âm thanh cho câu {CurrentQuestion.ID}")));
			});
			
			StopCmd = new RelayCommand(() =>
			{
				var res = MessageBox.Show("Bạn có muốn dừng khẩn cấp không?", "Dừng khẩn cấp", MessageBoxButton.YesNo, MessageBoxImage.Stop);
				if (res == MessageBoxResult.No)
					return;
				TimerStop();
				NetServer.SendCommandToAll(new KDCommand(CommandType.StopEmergency));
				Messenger.Default.Send(new ChangeState(ProgramState.Pending));
			});
			DoneCmd = new RelayCommand(() => { return; });
			#endregion
		}

		private void TimerStop()
		{
			Messenger.Default.Send(new ChangeState(ProgramState.Pending));
			Timer.Stop();
			TimerLabel = 0;
			if (!cancellation.IsCancellationRequested) cancellation.Cancel();
		}

		private void TimerChange(object sender, EventArgs e)
		{
			TimerLabel = TIME_LIMIT - (DateTime.Now - BeginingTime).TotalSeconds;
			if (TimerLabel <= 0)
				TimerStop();
		}

		private void TimerStart()
		{
			CurrentQuestion = GetNewQuestion(Data.StartQuestions);
			NetServer.SendCommandToAll(new KDCommand(CommandType.StartTimmer));
			TimerEnable = false;
			Messenger.Default.Send(new ChangeState(ProgramState.Playing));
			Messenger.Default.Send(new LogMess(KDLogger.Info($"Bắt đầu Khởi động cho {CurrentPlayer.Name}")));
			BeginingTime = DateTime.Now;
			Timer.Start();
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
			Messenger.Default.Send(new LogMess(KDLogger.Info($"ID: {tmp.ID}\nLĩnh vực: {tmp.SubjectName}\nCâu hỏi: {tmp.Content}\nĐáp án: {tmp.Answer}")));
			StoreItemCounter = source.Count;
			return tmp;
		}

		private void OutOfQuestion()
		{
			Messenger.Default.Send(new ChangeState(ProgramState.Pending));
			Messenger.Default.Send(new LogMess(KDLogger.Warn("Đã hểt câu hỏi!")));
			TimerStop();
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
			Messenger.Default.Send(new LogMess(KDLogger.Info($"Đúng, điểm của {CurrentPlayer.Name} là {CurrentPlayer.Score}", LogType.Player)));
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
			Messenger.Default.Send(new LogMess(KDLogger.Info($"Sai, điểm của {CurrentPlayer.Name} là {CurrentPlayer.Score}", LogType.Player)));
			NetServer.SendCommandToAll(new KDCommand(CommandType.Wrong));
		}

		private void CommandChecker(KDCommand command)
		{
			Task.Run(() =>
			{
				switch (command.PrefixCmd)
				{
					case CommandType.StartTimmer:
						TimerStart();
						break;
					case CommandType.NextQuestAt:
						CurrentQuestion = Data.StartQuestions[int.Parse(command.Content)];
						break;
					case CommandType.Right:
						CurrentPlayer.Score += 10;
						QuestCount++;
						break;
					case CommandType.Wrong:
						QuestCount++;
						break;
					case CommandType.StopEmergency:
						TimerStop();
						break;
					default:
						break;
				}
			});
		}
	}
}
