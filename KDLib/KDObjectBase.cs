using System.ComponentModel;

namespace KDLib
{
	public abstract class KDObjectBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public void NotifyPropertyChange(string name)
		{
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

		public void NotifyPropertyChange (params string[] names)
        {
			if (PropertyChanged != null)
            {
				foreach (var name in names)
                {
					PropertyChanged(this, new PropertyChangedEventArgs(name));
                }
            }
        }
	}
}
