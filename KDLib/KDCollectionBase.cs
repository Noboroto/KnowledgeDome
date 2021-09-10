using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace KDLib
{
	public abstract class KDCollectionBase<T> : ObservableCollection<T>
	{
		public KDCollectionBase() : base()
		{

		}
		public KDCollectionBase(IList<T> list) : base(list)
		{

		}
	}
}