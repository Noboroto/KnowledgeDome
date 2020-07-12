using System;
using System.IO;
using System.Text;

namespace KDLib
{
	public static class Data
	{
		private static int _CurrentPlayer;

		private static int _CurrentMatchIndex;

		private static MatchList _Matches;

		public static int CurrentPlayer
		{
			get
			{
				return _CurrentPlayer;
			}
			set
			{
				_CurrentPlayer = value;
			}
		}

		public static MatchList Matches
		{
			get
			{
				return _Matches;
			}
			set
			{
				_Matches = value;
			}
		}

		public static int CurrentMatchIndex
		{
			get
			{
				return _CurrentMatchIndex;
			}
			set
			{
				_CurrentMatchIndex = value;
			}
		}

		public static Match CurrentMatch => Matches[CurrentMatchIndex];

		/// <summary>
		/// (Làm sau) chuẩn bị dữ liệu
		/// </summary>
		public static void Initialize()
		{
			/*
			if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\Tests\\Images"))
			{
				Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Tests\\Images");
			}
			if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\Tests\\Clips"))
			{
				Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Tests\\Clips");
			}
			if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\Tests\\Sounds"))
			{
				Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Tests\\Sounds");
			}
			if (File.Exists("Matches.etai"))
			{
				Matches = new MatchList(new DataProvider(AIEncoder.GetString(File.ReadAllText("Matches.etai", Encoding.UTF8)), haskey: false));
			}
			else
			{
				Matches = new MatchList();
			}
			if (File.Exists("Tests\\Start.etai"))
			{
				StartQuestions = new StartQuestionList(new DataProvider(AIEncoder.GetString(File.ReadAllText("Tests\\Start.etai", Encoding.UTF8)), haskey: false));
			}
			else
			{
				StartQuestions = new StartQuestionList();
			}
			if (File.Exists("Tests\\Obstacle.etai"))
			{
				Obstacles = new ObstacleList(new DataProvider(AIEncoder.GetString(File.ReadAllText("Tests\\Obstacle.etai", Encoding.UTF8)), haskey: false));
			}
			else
			{
				Obstacles = new ObstacleList();
			}
			if (File.Exists("Tests\\Acceleration.etai"))
			{
				AccelerationQuestions = new AccelerationQuestionList(new DataProvider(AIEncoder.GetString(File.ReadAllText("Tests\\Acceleration.etai", Encoding.UTF8)), haskey: false));
			}
			else
			{
				AccelerationQuestions = new AccelerationQuestionList();
			}
			if (File.Exists("Tests\\Finish.etai"))
			{
				FinishQuestions = new FinishQuestionList(new DataProvider(AIEncoder.GetString(File.ReadAllText("Tests\\Finish.etai", Encoding.UTF8)), haskey: false));
			}
			else
			{
				FinishQuestions = new FinishQuestionList();
			}
			if (File.Exists("Tests\\Extra.etai"))
			{
				ExtraQuestions = new ExtraQuestionList(new DataProvider(AIEncoder.GetString(File.ReadAllText("Tests\\Extra.etai", Encoding.UTF8)), haskey: false));
			}
			else
			{
				ExtraQuestions = new ExtraQuestionList();
			}
			*/
		}

        /// <summary>
        /// Get ID for object
        /// </summary>
        /// <returns></returns>
        internal static int GenerateID()
        {
            return 0;
        }
    }
}
