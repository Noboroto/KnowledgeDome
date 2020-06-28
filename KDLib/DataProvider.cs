using System.Collections.Generic;

namespace KDLib
{
	public sealed class DataProvider
	{
		private char _Key;

		private string _Value;

		private List<DataProvider> _Children = new List<DataProvider>();

		public char Key
		{
			get
			{
				return _Key;
			}
			set
			{
				_Key = value;
			}
		}

		public string Value
		{
			get
			{
				return _Value;
			}
			set
			{
				_Value = value;
			}
		}

		public List<DataProvider> Children
		{
			get
			{
				return _Children;
			}
			set
			{
				_Children = value;
			}
		}

		public DataProvider(string data, bool haskey)
			: this("©", "®", data, haskey)
		{
		}

		public DataProvider(string openningsign, string closingsign, string data, bool haskey)
		{
			if (haskey)
			{
				Key = data[1];
				data = data.Substring(2, data.Length - 4);
			}
			if (!data.Contains(openningsign))
			{
				Value = data;
			}
			while (data.Contains(openningsign))
			{
				char c = data[1];
				Children.Add(new DataProvider(openningsign, closingsign, data.Substring(0, data.IndexOf(closingsign + c.ToString(), 2) + 2), haskey: true));
				if (data.IndexOf(closingsign + c.ToString(), 2) != data.LastIndexOf(closingsign))
				{
					data = data.Substring(data.IndexOf(closingsign + c.ToString(), 2) + 2);
					continue;
				}
				break;
			}
		}

		public bool ExistsChild(char key)
		{
			return Children.Find((DataProvider ctf) => ctf.Key == key) != null;
		}

		public DataProvider GetChild(char key)
		{
			return Children.Find((DataProvider ctf) => ctf.Key == key);
		}

		public List<DataProvider> GetChildren(char key)
		{
			return Children.FindAll((DataProvider ctf) => ctf.Key == key);
		}

		public string GetChildValue(char key)
		{
			return GetChild(key).Value;
		}
	}
}
