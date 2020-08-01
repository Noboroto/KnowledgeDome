using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KDLib
{
	public abstract class KDCollectionBase<T> : ObservableCollection<T>
	{
		public abstract bool Contains(object id);
	}
}