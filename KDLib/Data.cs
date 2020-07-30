using System;
using System.Collections.Generic;
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
        private static event EventHandler<PropertyChangedEventArgs> StaticPropertiesChanged;
        #endregion

        #region PublicProperties

        public static int _CurrentMatchIndex;

        public static MachineType ThisMacineType { get; set; }

        public static bool OnFocus { get; set; }

        public static List<string> ListIP { get; private set; }

        public static KDCommandList Commands { get; set; }

        public static int CurrentMatchIndex 
        {
            get
            {
                return _CurrentMatchIndex;
            }
            set
            {
                _CurrentMatchIndex = value;
                NotifyStaticPropertyChanged();
            }
        }

        public static int CurrentPlayer { get; set; }

        public static MatchList Matches { get; set; }
        #endregion

        /// <summary>
        /// (Làm sau) chuẩn bị dữ liệu
        /// </summary>
        public static void Initialize()
		{
            Commands = new KDCommandList();
        }

        private static void NotifyStaticPropertyChanged ([CallerMemberName] string propertyName = "")
        {
            StaticPropertiesChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }

        private static void NotifyStaticPropertyChanged(params string[] Names)
        {
            if (StaticPropertiesChanged != null)
            {
                foreach (var propertyName in Names)
                {
                    StaticPropertiesChanged(null, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
    }
}
