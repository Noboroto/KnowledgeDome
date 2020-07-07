using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace KDLib
{
	public abstract class KDCollectionBase<T> : List<T>, INotifyPropertyChanged, INotifyCollectionChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public new virtual void Add(T item)
		{
			base.Add(item);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
		}

		public new virtual void Remove(T item)
		{
			base.Remove(item);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
		}

		public new virtual void Sort(Comparison<T> comparer)
		{
			base.Sort(comparer);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		public virtual bool Contains (int id)
        {
			return false;
        }

		internal void OnPropertyChanged(string propertyname)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
			}
		}

		internal void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, args);
				OnPropertyChanged("Count");
			}
		}
	}
}