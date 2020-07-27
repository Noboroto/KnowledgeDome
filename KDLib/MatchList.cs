using System.Collections.Specialized;
using System.ComponentModel;

namespace KDLib
{
	public class MatchList : KDCollectionBase<Match>
	{
		public override void Add(Match item)
		{
			base.Add(item);
			item.PropertyChanged += Item_PropertyChanged;
		}

		public override void Remove(Match item)
		{
			item.PropertyChanged -= Item_PropertyChanged;
			base.Remove(item);
		}

		public MatchList()
		{
		}

		public override bool Contains(object id)
		{
			return false;
		}

		private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}
	}
}
