using Newtonsoft.Json;
using System;

namespace KDLib
{
	public class AccelerationQuestionList : KDCollectionBase<AccelerationQuestion>
	{
		[JsonIgnore]
		private int IQCount => FindAll((AccelerationQuestion q) => q.HintImages.Count == 1).Count;

		[JsonIgnore]
		private int ImageStringCount => FindAll((AccelerationQuestion q) => q.HintImages.Count > 2).Count;

		[JsonIgnore]
		private int JigsawCount => FindAll((AccelerationQuestion q) => q.HintImages.Count == 2).Count;

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
