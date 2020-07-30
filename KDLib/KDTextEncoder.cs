namespace KDLib
{
    public class KDTextEncoder
    {
		#region PrivateMembers
		#endregion

		#region PublicProperties
		#endregion
		public static string GetCode(string rawdata)
		{
			string text = "";
			foreach (char c in rawdata)
			{
				text += ((char)(ushort)(c + 1372)).ToString();
			}
			return text;
		}

		public static string GetString(string rawdata)
		{
			string text = "";
			foreach (char c in rawdata)
			{
				text += ((char)(ushort)(c - 1372)).ToString();
			}
			return text;
		}
	}
}
