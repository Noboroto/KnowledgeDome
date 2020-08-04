using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
        public static MachineType ThisMacineType { get; set; }

        public static bool OnFocus { get; set; }

        public static List<string> ListIP { get; private set; }

        public static KDCommandList Commands { get; set; }

        public static int CurrentMatchIndex { get; set; }

        public static int CurrentPlayer { get; set; }

        public static MatchList Matches { get; set; }
        #endregion
        /// <summary>
        /// (Làm sau) chuẩn bị dữ liệu
        /// </summary>
        public static void Initialize()
		{
            Commands = new KDCommandList();
            NetServer.Initialize();
            NetClient.Initialize();
        }

        public static void InitializeForDevelop()
        {

        }
    }
}
