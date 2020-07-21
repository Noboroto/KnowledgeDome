namespace KDLib
{
	public static class Data
	{
		private static MachineType _ThisMachine;

		private static bool _OnFocus;

		private static int _CurrentPlayer;

		private static int _CurrentMatchIndex;

		private static MatchList _Matches;

        public const int PortForTCP = 2644;

		public const int PortForChecker = 2645;

		public const int PortForValidCheck = 2647;

		public static MachineType ThisMacineType
        {
            get
            {
				return _ThisMachine;
            }
            set
            {
				_ThisMachine = (MachineType)value;
            }
        }

		public static bool OnFocus
        {
			get
            {
				return _OnFocus;
            }
			set
            {
				_OnFocus = value;
            }
        }

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

		/// <summary>
		/// (Làm sau) chuẩn bị dữ liệu
		/// </summary>
		public static void Initialize()
		{

		}
    }
}
