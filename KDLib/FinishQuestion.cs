using Newtonsoft.Json;

namespace KDLib
{
	public class FinishQuestion : Question
	{
        #region PrivateMembers
        private int _Value;
        #endregion

        #region PublicProperties
        public int Value
        {
            get
            {
				return _Value;
            }
			set
            {
                Set(nameof(Value), ref _Value, value);
            }
        }
        #endregion

        [JsonConstructor]
		public FinishQuestion(int id, int value, string content, string answer)
			: base(id, content, answer)
		{
			Value = value;
		}
	}
}
