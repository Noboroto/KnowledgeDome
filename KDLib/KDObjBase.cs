using System.ComponentModel;

namespace KDLib
{
	public abstract class KDObjBase : INotifyPropertyChanged
	{

		public event PropertyChangedEventHandler PropertyChanged;

		internal void OnPropertyChanged(string name)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}
	}
}
