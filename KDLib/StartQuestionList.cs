using Newtonsoft.Json;

namespace KDLib
{
	public class StartQuestionList : KDCollectionBase<StartQuestion>
	{
		[JsonIgnore]
		public int MathCount => FindAll((StartQuestion sq) => sq.Subject == SubjectInfo.Math).Count;

		[JsonIgnore]
		public int PhysicsCount => FindAll((StartQuestion sq) => sq.Subject == SubjectInfo.Physics).Count;

		[JsonIgnore]
		public int ChemistryCount => FindAll((StartQuestion sq) => sq.Subject == SubjectInfo.Physics).Count;

		[JsonIgnore]
		public int BiologyCount => FindAll((StartQuestion sq) => sq.Subject == SubjectInfo.Biology).Count;

		[JsonIgnore]
		public int LiteratureCount => FindAll((StartQuestion sq) => sq.Subject == SubjectInfo.Literature).Count;

		[JsonIgnore]
		public int HistoryCount => FindAll((StartQuestion sq) => sq.Subject == SubjectInfo.History).Count;

		[JsonIgnore]
		public int GeographyCount => FindAll((StartQuestion sq) => sq.Subject == SubjectInfo.Geography).Count;

		[JsonIgnore]
		public int SportCount => FindAll((StartQuestion sq) => sq.Subject == SubjectInfo.Sport).Count;

		[JsonIgnore]
		public int ArtsCount => FindAll((StartQuestion sq) => sq.Subject == SubjectInfo.Arts).Count;

		[JsonIgnore]
		public int OthersCount => FindAll((StartQuestion sq) => sq.Subject == SubjectInfo.Other).Count;

		[JsonIgnore]
		public int GeneralCount => FindAll((StartQuestion sq) => sq.Subject == SubjectInfo.General).Count;

		[JsonIgnore]
		public int EnglishCount => FindAll((StartQuestion sq) => sq.Subject == SubjectInfo.English).Count;

		public StartQuestionList()
		{

		}

		public override void Add(StartQuestion sq)
		{
			base.Add(sq);
		}

		public override void Remove(StartQuestion sq)
		{
			base.Remove(sq);
		}

		public override bool Contains(object id)
		{
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ID == (int)id)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
