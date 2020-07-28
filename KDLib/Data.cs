using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;

namespace KDLib
{
	public static class Data
	{
        public const string KeyMC = "MC";

        public const string KeyViewer = "Viewer";

        public const int PortForTCP = 2644;

		public const int PortForChecker = 2645;

		public const int PortForValidCheck = 2647;
        
        public static MachineType ThisMacineType { get; set; }

        public static bool OnFocus { get; set; }

        public static List<string> ListIP { get; private set; }

        public static KDCommandList Commands { get; set; }

        public static int CurrentMatchIndex { get; set; }

        public static int CurrentPlayer { get; set; }

        public static MatchList Matches { get; set; }

        /// <summary>
        /// (Làm sau) chuẩn bị dữ liệu
        /// </summary>
        public static void Initialize()
		{
            Commands = new KDCommandList();
        }
    }
}
