using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KDLib
{
	public class AccelerationQuestionList : KDCollectionBase<AccelerationQuestion>
	{
		[JsonIgnore]
		public int IQCount => FindAll((AccelerationQuestion q) => q.HintImages.Count == 1).Count;

		[JsonIgnore]
		public int ImageStringCount => FindAll((AccelerationQuestion q) => q.HintImages.Count > 2).Count;

		[JsonIgnore]
		public int JigsawCount => FindAll((AccelerationQuestion q) => q.HintImages.Count == 2).Count;

		public AccelerationQuestionList()
		{
		}

		public override void Add(AccelerationQuestion item)
		{
			base.Add(item);
			CallCountProperty(item);
		}

		public override void Remove(AccelerationQuestion item)
		{
			base.Remove(item);
			CallCountProperty(item);
		}

		public void Edit(AccelerationQuestion q1, AccelerationQuestion q2)
		{
			int count = q1.HintImages.Count;
			q1.GetValueFrom(q2);
			if (count != q1.HintImages.Count)
			{
				CallCountProperty(count);
				CallCountProperty(q1);
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
		/// Cài sau
		/// </summary>
		public void Save()
		{
			//Cài sau
			/* string text = "";
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AccelerationQuestion current = enumerator.Current;
					text += current.TextData();
				}
			}
			File.WriteAllText("Tests\\Acceleration.etai", AIEncoder.GetCode(text), Encoding.UTF8);*/
		}

		public AccelerationQuestion FindFromID(int id)
		{
			return Find((AccelerationQuestion aq) => aq.ID == id);
		}

		private void CallCountProperty(AccelerationQuestion item)
		{
			CallCountProperty(item.HintImages.Count);
		}

		private void CallCountProperty(int imagecount)
		{
			switch (imagecount)
			{
				case 1:
					OnPropertyChanged("IQCount");
					break;
				case 2:
					OnPropertyChanged("JigsawCount");
					break;
				default:
					OnPropertyChanged("ImageStringCount");
					break;
			}
		}
	}
}
