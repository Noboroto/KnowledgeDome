using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KDLib
{
	public class StartQuestionList : KDCollectionBase<StartQuestion>
	{
		public int MathCount => FindAll((StartQuestion sq) => sq.Subject == SubjectInfo.Math).Count;

		public int PhysicsCount => FindAll((StartQuestion sq) => sq.Subject == SubjectInfo.Physics).Count;

		public int ChemistryCount => FindAll((StartQuestion sq) => sq.Subject == SubjectInfo.Physics).Count;

		public int BiologyCount => FindAll((StartQuestion sq) => sq.Subject == SubjectInfo.Biology).Count;

		public int LiteratureCount => FindAll((StartQuestion sq) => sq.Subject == SubjectInfo.Literature).Count;

		public int HistoryCount => FindAll((StartQuestion sq) => sq.Subject == SubjectInfo.History).Count;

		public int GeographyCount => FindAll((StartQuestion sq) => sq.Subject == SubjectInfo.Geography).Count;

		public int SportCount => FindAll((StartQuestion sq) => sq.Subject == SubjectInfo.Sport).Count;

		public int ArtsCount => FindAll((StartQuestion sq) => sq.Subject == SubjectInfo.Arts).Count;

		public int OthersCount => FindAll((StartQuestion sq) => sq.Subject == SubjectInfo.Other).Count;

		public int GeneralCount => FindAll((StartQuestion sq) => sq.Subject == SubjectInfo.General).Count;

		public int EnglishCount => FindAll((StartQuestion sq) => sq.Subject == SubjectInfo.English).Count;

		public StartQuestionList()
		{

		}

		public override void Add(StartQuestion sq)
		{
			base.Add(sq);
			OnPropertyChanged(sq.Subject.ToString() + "Count");
		}

		public override void Remove(StartQuestion sq)
		{
			base.Remove(sq);
			OnPropertyChanged(sq.Subject.ToString()+ "Count");
		}

		public void Edit(int index, string subject, string content, string answer)
		{
			/* EDIT LATER
			string subject2 = base[index].Subject;
			base[index].Subject = subject;
			base[index].Content = content;
			base[index].Answer = answer;
			if (subject2 != subject)
			{
				OnPropertyChanged(GetChildListName(subject2) + "Count");
				OnPropertyChanged(GetChildListName(subject) + "Count");
			}
			*/
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

		public void Save()
		{
			//Bổ sung sau
			//File.WriteAllText("Tests\\Start.etai", KDTextEncoder.GetCode(text), Encoding.UTF8);
		}

		public StartQuestion SearchFromID(int id)
		{
			return Find((StartQuestion sq) => sq.ID == id);
		}
	}
}
