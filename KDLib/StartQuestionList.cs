using Newtonsoft.Json;

using System.Linq;

namespace KDLib
{
    public class StartQuestionList : KDCollectionBase<StartQuestion>
    {
        [JsonIgnore]
        public int MathCount => this.Where((StartQuestion sq) => sq.Subject == SubjectInfo.Math).Count();

        [JsonIgnore]
        public int PhysicsCount => this.Where((StartQuestion sq) => sq.Subject == SubjectInfo.Physics).Count();

        [JsonIgnore]
        public int ChemistryCount => this.Where((StartQuestion sq) => sq.Subject == SubjectInfo.Physics).Count();

        [JsonIgnore]
        public int BiologyCount => this.Where((StartQuestion sq) => sq.Subject == SubjectInfo.Biology).Count();

        [JsonIgnore]
        public int LiteratureCount => this.Where((StartQuestion sq) => sq.Subject == SubjectInfo.Literature).Count();

        [JsonIgnore]
        public int HistoryCount => this.Where((StartQuestion sq) => sq.Subject == SubjectInfo.History).Count();

        [JsonIgnore]
        public int GeographyCount => this.Where((StartQuestion sq) => sq.Subject == SubjectInfo.Geography).Count();

        [JsonIgnore]
        public int SportCount => this.Where((StartQuestion sq) => sq.Subject == SubjectInfo.Sport).Count();

        [JsonIgnore]
        public int ArtsCount => this.Where((StartQuestion sq) => sq.Subject == SubjectInfo.Arts).Count();

        [JsonIgnore]
        public int OthersCount => this.Where((StartQuestion sq) => sq.Subject == SubjectInfo.Other).Count();

        [JsonIgnore]
        public int GeneralCount => this.Where((StartQuestion sq) => sq.Subject == SubjectInfo.General).Count();

        [JsonIgnore]
        public int EnglishCount => this.Where((StartQuestion sq) => sq.Subject == SubjectInfo.English).Count();

        public StartQuestionList()
        {

        }

        public override bool Contains(object id)
        {
            using (var enumerator = GetEnumerator())
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
