using System.Collections.Specialized;
using System.ComponentModel;

namespace KDLib
{
	public class MatchList : KDCollectionBase<Match>
	{
		public override void Add(Match item)
		{
			base.Add(item);
			item.PropertyChanged += Item_PropertyChanged;
		}

		public override void Remove(Match item)
		{
			item.PropertyChanged -= Item_PropertyChanged;
			base.Remove(item);
		}

		public MatchList()
		{
		}

		/// <summary>
		/// Để cài sau
		/// </summary>

		public void Save()
		{
			/*string text = "";
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Match current = enumerator.Current;
					text += current.TextData();
				}
			}
			File.WriteAllText("Matches.etai", AIEncoder.GetCode(text), Encoding.UTF8);*/
		}

		public override bool Contains(object id)
		{
			return false;
		}

		private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}
	}
}
