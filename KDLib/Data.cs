using Newtonsoft.Json;

using System.IO;

namespace KDLib
{
	public static class Data
	{
		#region PublicConstants
		public const int PortForTCP = 2644;

		public const int PortForChecker = 2645;

		public const int PortForValidCheck = 2647;
		#endregion

		#region PrivateMembers
		#endregion

		#region PublicProperties
		public static ProgramState Status { get; set; }
		public static int ID { get; set; }
		public static string ChooseIP { get; set; }
		public static int Pos { get; set; }
		public static Machine ThisMacineType
		{
			get
			{
				if (ID >= 0 && ID < CurrentMatch.Players.Count) return Machine.Player;
				else if (ID == -2) return Machine.None;
				else if (ID == -1) return Machine.Server;
				else if (ID == CurrentMatch.Players.Count) return Machine.MC;
				return Machine.Viewer;
			}
		}
		public static int CurrentRound { get; set; }
		public static bool OnFocus { get; set; }
		public static KDCommandList NetCommands { get; set; }
		public static KDCommandList FrameCommands { get; set; }
		public static KDCommandList RoundCommnads { get; set; }
		public static int CurrentMatchIndex { get; set; }
		public static int CurrentPlayerIndex { get; set; }
		public static MatchInfo CurrentMatch => MatchInfos[CurrentMatchIndex];
		public static Player CurrentPlayer
		{
			get => MatchInfos[CurrentMatchIndex].Players[CurrentPlayerIndex];
			set => MatchInfos[CurrentMatchIndex].Players[CurrentPlayerIndex] = value;
		}
		public static MatchInfoList MatchInfos { get; set; }
		public static StartQuestionList StartQuestions { get; set; }
		public static ObstacleList Obstacles { get; set; }
		public static AccelerationQuestionList AccelerationQuestions { get; set; }
		public static FinishQuestionList FinishQuestions { get; set; }
		public static ExtraQuestionList ExtraQuestions { get; set; }
		#endregion
		/// <summary>
		/// (Làm sau) chuẩn bị dữ liệu
		/// </summary>
		public static void ServerInitialize()
		{
			ID = -1;
			Initialize();
			NetServer.Initialize();
		}

		public static void ClientInitialize()
		{
			ID = -2;
			Initialize();
			NetClient.Initialize();
		}

		public static string ToJson<T>(T o)
		{
			return JsonConvert.SerializeObject(o);
		}

		public static T FromJosn<T>(string source)
		{
			return JsonConvert.DeserializeObject<T>(source);
		}

		public static Machine GetMachineFromID(int id)
		{
			if (id >= 0 && id < CurrentMatch.Players.Count) return Machine.Player;
			else if (id == -2) return Machine.None;
			else if (id == -1) return Machine.Server;
			else if (id == CurrentMatch.Players.Count) return Machine.MC;
			return Machine.Viewer;
		}

		private static void Initialize()
		{
			NetCommands = new KDCommandList();
			RoundCommnads = new KDCommandList();
			FrameCommands = new KDCommandList();
			CurrentMatchIndex = 0;
			CurrentPlayerIndex = 0;
			KDLogger.Initialize();
			if (!Directory.Exists(@"Tests\")) Directory.CreateDirectory("Tests");
			StartQuestions = StartQuestionList.ReadFromFile();
			Obstacles = ObstacleList.ReadFromFile();
			AccelerationQuestions = AccelerationQuestionList.ReadFromFile();
			FinishQuestions = FinishQuestionList.ReadFromFile();
			ExtraQuestions = ExtraQuestionList.ReadFromFile();
			MatchInfos = MatchInfoList.ReadFromFile();
		}
	}
}
