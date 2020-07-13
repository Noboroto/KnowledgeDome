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

		/// <summary>
		/// (Làm sau) chuẩn bị dữ liệu
		/// </summary>
		public static void Initialize()
		{

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
