using Newtonsoft.Json;
using System;

namespace KDLib
{
	public class AccelerationQuestionList : KDCollectionBase<AccelerationQuestion>
	{
        #region PrivateMembers
        #endregion

        #region PublicProperties
        #endregion

        public AccelerationQuestionList()
		{

		}

		public override bool Contains(object id)
		{
			using (var enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ID == (int)id)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
