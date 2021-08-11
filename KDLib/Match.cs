using GalaSoft.MvvmLight;

using Newtonsoft.Json;

namespace KDLib
{
	public class Match : ObservableObject
	{
		#region PrivateMembers
		#endregion

		#region PublicProperties
		public StartQuestionList StartQuestions { get; set; }

		public ObstacleList Obstacles { get; set; }

		public AccelerationQuestionList AccelerationQuestions { get; set; }

		public FinishQuestionList FinishQuestions { get; set; }

		public ExtraQuestionList ExtraQuestions { get; set; }

		public string Name { get; private set; }

		public PlayerList Players { get; set; }
		#endregion

		public Match()
		{
			Players = new PlayerList();
			StartQuestions = new StartQuestionList();
			Obstacles = new ObstacleList();
			AccelerationQuestions = new AccelerationQuestionList();
			FinishQuestions = new FinishQuestionList();
			ExtraQuestions = new ExtraQuestionList();
		}

		[JsonConstructor]
		public Match(PlayerList players, StartQuestionList starts, ObstacleList obstacles, AccelerationQuestionList accelerations, FinishQuestionList finishes, ExtraQuestionList extras)
		{
			Players = players;
			StartQuestions = starts;
			Obstacles = obstacles;
			AccelerationQuestions = accelerations;
			FinishQuestions = finishes;
			ExtraQuestions = extras;
		}

		public Match(string name)
			: this()
		{
			Name = name;
		}
	}
}
