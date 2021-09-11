using Newtonsoft.Json;

namespace KDLib
{
	public class DataPackage
	{
		#region PublicProperties
		public ProgramState Status { get; private set; }
		public int CurrentRound { get; private set; }
		public int CurrentMatchIndex { get; private set; }
		public int CurrentPlayerIndex { get; private set; }
		#endregion
		
		[JsonConstructor]
		public DataPackage(ProgramState status, int currentround, int currentmatchindex, int currentplayerindex)
		{
			Status = status;
			CurrentRound = currentround;
			CurrentMatchIndex = currentmatchindex;
			CurrentPlayerIndex = currentplayerindex;
		}

		public static DataPackage GetPackage()
		{
			return new DataPackage(Data.Status, KDConvert.UrlStringToID(Data.CurrentPage.ToString()), Data.CurrentMatchIndex, Data.CurrentPlayerIndex);
		}
	}
}
