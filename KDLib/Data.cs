using System.Collections.Generic;
using System.Windows.Media;

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
		public static Match CurrentMatch { get; set; }
		public static MatchList Matches { get; set; }
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
			CurrentMatch = new Match
			{
				Players = new PlayerList()
			};
		}
		public static void InitializeForDevelop()
		{
			ThisMacineType = MachineType.Server;
			Initialize();
			CurrentPlayerIndex = 0;
			CurrentMatch.Players.Add(new Player(12, "asdasd")
			{
				BackgroundColor = (Brush)new BrushConverter().ConvertFromString(@"#16acea"),
				ForegroundColor = Brushes.Black
			});
			for (int i = 0; i < 100; ++i)
			{
				CurrentMatch.StartQuestions.Add(new StartQuestion("Toán học", "Toán học " + i, "no", i));
			};
		}
	}
}
