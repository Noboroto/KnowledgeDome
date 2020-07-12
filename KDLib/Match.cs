using Newtonsoft.Json;
using System.Collections.Specialized;

namespace KDLib
{
	public class Match : KDObjectBase
	{
		private string _Name;

		private PlayerList _Players;

		private static StartQuestionList _StartQuestions;

		private static ObstacleList _Obstacles;

		private static AccelerationQuestionList _AccelerationQuestions;

		private static FinishQuestionList _FinishQuestions;

		private static ExtraQuestionList _ExtraQuestions;

		public static StartQuestionList StartQuestions
		{
			get
			{
				return _StartQuestions;
			}
			set
			{
				_StartQuestions = value;
			}
		}

		public static ObstacleList Obstacles
		{
			get
			{
				return _Obstacles;
			}
			set
			{
				_Obstacles = value;
			}
		}

		public static AccelerationQuestionList AccelerationQuestions
		{
			get
			{
				return _AccelerationQuestions;
			}
			set
			{
				_AccelerationQuestions = value;
			}
		}

		public static FinishQuestionList FinishQuestions
		{
			get
			{
				return _FinishQuestions;
			}
			set
			{
				_FinishQuestions = value;
			}
		}

		public static ExtraQuestionList ExtraQuestions
		{
			get
			{
				return _ExtraQuestions;
			}
			set
			{
				_ExtraQuestions = value;
			}
		}

		public string Name
		{
			get
			{
				return _Name;
			}
			private set
			{
				_Name = value;
				OnPropertyChanged("Name");
			}
		}

		public PlayerList Players
		{
			get
			{
				return _Players;
			}
			set
			{
				_Players = value;
				OnPropertyChanged("Players");
			}
		}

		public Match()
		{
			Players = new PlayerList();
			StartQuestions = new StartQuestionList();
			Obstacles = new ObstacleList();
			AccelerationQuestions = new AccelerationQuestionList();
			FinishQuestions = new FinishQuestionList();
			ExtraQuestions = new ExtraQuestionList();
			//Players.CollectionChanged += Players_CollectionChanged;
		}

		[JsonConstructor]
		public Match(string name)
			: this()
		{
			Name = name;
		}

		private void Players_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			OnPropertyChanged("Players");
		}
	}
}
