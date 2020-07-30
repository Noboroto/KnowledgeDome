using Newtonsoft.Json;

namespace KDLib
{
	public class ExtraQuestion : Question
	{
        #region PrivateMembers
        #endregion

        #region PublicProperties
        #endregion

        [JsonConstructor]
		public ExtraQuestion(int id, string question, string answer)
			: base(id,question, answer)
		{
		}
	}
}
