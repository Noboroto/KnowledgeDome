using Newtonsoft.Json;

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
		#endregion

		#region PrivateMembers
		#endregion

		#region PublicProperties
		public static string ChooseIP { get; set; }
		public static Machine ThisMacineType { get; set; }
		public static bool OnFocus { get; set; }
		public static KDCommandList Commands { get; set; }
		public static int CurrentMatchIndex { get; set; }
		public static int CurrentPlayerIndex { get; set; }
		public static MatchInfo CurrentMatch => MatchInfos[CurrentMatchIndex];
		public static Player CurrentPlayer => MatchInfos[CurrentMatchIndex].Players[CurrentPlayerIndex];
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
			ThisMacineType = Machine.Server;
			Initialize();
			NetServer.Initialize();
		}

		public static void ClientInitialize()
		{
			ThisMacineType = Machine.None;
			NetClient.Initialize();
		}

		public static string ToJson<T>(T o)
		{
			return JsonConvert.SerializeObject(o);
		}

		public static T FromJosn<T> (string source)
		{
			return JsonConvert.DeserializeObject<T>(source);
		}

		private static void Initialize()
		{
			CurrentMatchIndex = 0;
			CurrentPlayerIndex = 0;
			KDLogger.Initialize();
			Commands = new KDCommandList();
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
