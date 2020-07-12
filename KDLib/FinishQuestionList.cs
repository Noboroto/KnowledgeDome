using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KDLib
{
	public class FinishQuestionList : KDCollectionBase<FinishQuestion>
	{
		[JsonIgnore]
		public int P10Count => FindAll((FinishQuestion q) => q.Value == 10).Count;

		[JsonIgnore]
		public int P20Count => FindAll((FinishQuestion q) => q.Value == 20).Count;

		[JsonIgnore]
		public int P30Count => FindAll((FinishQuestion q) => q.Value == 30).Count;

		public FinishQuestionList()
		{
		}

		public override void Add(FinishQuestion item)
		{
			base.Add(item);
			OnPropertyChanged("P" + item.Value.ToString() + "Count");
		}

		public override void Remove(FinishQuestion item)
		{
			base.Remove(item);
			OnPropertyChanged("P" + item.Value.ToString() + "Count");
		}

		public void Edit(FinishQuestion q1, FinishQuestion q2)
		{
			int value = q1.Value;
			q1.GetValueFrom(q2);
			if (value != q1.Value)
			{
				OnPropertyChanged("P" + value.ToString() + "Count");
				OnPropertyChanged("P" + q1.Value.ToString() + "Count");
			}
		}

		public override bool Contains(int id)
		{
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ID == id)
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Làm sau
		/// </summary>
		public void Save()
		{
			/*
			string text = "";
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FinishQuestion current = enumerator.Current;
					text += current.TextData();
				}
			}
			File.WriteAllText("Tests\\Finish.etai", AIEncoder.GetCode(text), Encoding.UTF8);*/
		}

		public FinishQuestion FromHashcode(int id)
		{
			return Find((FinishQuestion fq) => fq.ID == id);
		}
	}
}
