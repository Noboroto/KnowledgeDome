using System.Collections.Generic;
using System.IO;

namespace KDLib
{
	public static class Data
	{
		#region PublicConstants
		public const string KeyMC = "MC";

		public const string KeyViewer = "Viewer";

		public const int PortForTCP = 2644;

		public const int PortForChecker = 2645;

		public const int PortForValidCheck = 2647;

		public static readonly List<string> NameMachine = new List<string>
		{
			"Thí sinh",
			"MC",
			"Khán giả",
		};
		#endregion

		#region PrivateMembers
		#endregion

		#region PublicProperties
		public static string ChooseIP { get; set; }
		public static MachineType ThisMacineType { get; set; }
		public static bool OnFocus { get; set; }
		public static List<string> ListIP { get; private set; }
		public static KDCommandList Commands { get; set; }
		public static int CurrentMatchIndex { get; set; }
		public static int CurrentPlayerIndex { get; set; }
		public static MatchInfo CurrentMatch { get; set; }
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
			ThisMacineType = MachineType.Server;
			Initialize();
			NetServer.Initialize();
		}

		public static void ClientInitialize()
		{
			Commands = new KDCommandList();
			NetClient.Initialize();
		}

		public static void Initialize()
		{
			Commands = new KDCommandList();
			ListIP = NetServer.GetLocalIPAddress();
			if (!Directory.Exists(@"Tests\")) Directory.CreateDirectory("Tests");
			StartQuestions = new StartQuestionList();
			Obstacles = new ObstacleList();
			AccelerationQuestions = new AccelerationQuestionList();
			FinishQuestions = new FinishQuestionList();
			ExtraQuestions = new ExtraQuestionList();
			MatchInfos = new MatchInfoList();
		}
	}
}
