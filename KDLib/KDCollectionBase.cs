using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KDLib
{
	public abstract class KDCollectionBase<T> : List<T>, INotifyPropertyChanged, INotifyCollectionChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public new virtual void Add(T item)
		{
			base.Add(item);
			NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
		}

		public new virtual void Remove(T item)
		{
			base.Remove(item);
			NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
		}

		public new virtual void Sort(Comparison<T> comparer)
		{
			base.Sort(comparer);
			NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		public abstract bool Contains(object id);

		public void NotifyPropertyChanged([CallerMemberName] string name = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		public void NotifyPropertyChanged(params string[] names)
		{
			if (PropertyChanged != null)
			{
				foreach (var name in names)
				{
					PropertyChanged(this, new PropertyChangedEventArgs(name));
				}
			}
		}

		internal void NotifyCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			if (this.CollectionChanged != null)
			{
				CollectionChanged(this, args);
				NotifyPropertyChanged(nameof(Count));
			}
		}
	}
}