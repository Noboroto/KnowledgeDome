using GalaSoft.MvvmLight;
using KDLib;

using System.Windows.Media;

namespace KDCtrlLib.ViewModels
{
	public class StartRoundViewPlayerViewModel : ViewModelBase
	{
		private Player _CurrentPlayer;
		private StartQuestion _CurrentQuestion;
		private int QuestCount;

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
		public StartRoundViewPlayerViewModel()
		{
			var b = new Player(12, "asdasd")
			{
				BackgroundColor = (Brush)new BrushConverter().ConvertFromString(@"#16acea"),
				ForegroundColor = Brushes.Black
			};
			CurrentPlayer = b;
		}
	}
}
