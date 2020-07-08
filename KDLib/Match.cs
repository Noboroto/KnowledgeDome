using System.Collections.Specialized;

namespace KDLib
{
	public class Match : KDObjectBase
	{
		private string _Name;

		private PlayerList _Players;

		public string Name
		{
			get
			{
				return _Name;
			}
			private set
			{
				_Name = value;
				OnPropertyChanged("Name");
			}
		}

		public PlayerList Players
		{
			get
			{
				return _Players;
			}
			set
			{
				_Players = value;
				OnPropertyChanged("Players");
			}
		}

		public Match()
		{
			Players = new PlayerList();
			Players.CollectionChanged += Players_CollectionChanged;
		}

		public Match(string name)
			: this()
		{
			Name = name;
		}

		public void GetValueFrom(Match m)
		{
			Name = m.Name;
		}

		private void Players_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			OnPropertyChanged("Players");
		}
	}
}
