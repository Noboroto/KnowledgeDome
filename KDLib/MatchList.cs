using System.Collections.Specialized;
using System.ComponentModel;

namespace KDLib
{
	public class MatchList : KDCollectionBase<Match>
	{
		public MatchList()
		{
		}

		public override bool Contains(object id)
		{
			return false;
		}
	}
}
