using System.Collections.ObjectModel;

namespace KDLib
{
	public abstract class KDCollectionBase<T> : ObservableCollection<T>
	{
		public abstract bool Contains(object id);
	}
}