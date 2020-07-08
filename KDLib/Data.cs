using System;
using System.IO;
using System.Text;

namespace KDLib
{
	public static class Data
	{
		private static MatchList _Matches;

		private static StartQuestionList _StartQuestions;

		private static ObstacleList _Obstacles;

		private static AccelerationQuestionList _AccelerationQuestions;

		private static FinishQuestionList _FinishQuestions;

		private static ExtraQuestionList _ExtraQuestions;

		private static int _CurrentMatchIndex;

		private static int _CurrentPlayer;

		private static Random Generator = new Random();

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

		internal static bool Contains(int ID)
		{
			if (StartQuestions != null && StartQuestions.Contains(ID) && Obstacles != null && Obstacles.Contains(ID) && AccelerationQuestions != null && AccelerationQuestions.Contains(ID) && FinishQuestions != null && FinishQuestions.Contains(ID) && ExtraQuestions != null && ExtraQuestions.Contains(ID))
			{
				if (Matches != null)
				{
					return Matches.Contains(ID);
				}
				return false;
			}
			return false;
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
