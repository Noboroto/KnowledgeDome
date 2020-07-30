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

		public override void Add(AccelerationQuestion item)
		{
			base.Add(item);
		}

		public override void Remove(AccelerationQuestion item)
		{
			base.Remove(item);
		}

		public override bool Contains(object id)
		{
			using (Enumerator enumerator = GetEnumerator())
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
