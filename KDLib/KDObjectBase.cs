using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KDLib
{
	public abstract class KDObjectBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public void NotifyPropertyChanged([CallerMemberName] string name = "")
		{
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

		public void NotifyPropertyChanged (params string[] names)
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
