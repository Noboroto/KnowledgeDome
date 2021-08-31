using GalaSoft.MvvmLight;
using KDLib;

using System.Windows.Media;

namespace KDCtrlLib.ViewModels
{
	public class StartRoundViewPlayerViewModel : ViewModelBase
	{
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


		public StartRoundViewPlayerViewModel()
		{
			DebugMethod();
		}

		private void DebugMethod ()
		{
			CurrentPlayer = new Player(12, "asdasd")
			{
				BackgroundColor = (Brush)new BrushConverter().ConvertFromString(@"#16acea"),
				ForegroundColor = Brushes.Black
			};
			CurrentQuestion = new StartQuestion("Toán học", "abc", "ad", 0);
			QuestCount = 2;
			Timer = 23.5;
		}
	}
}
