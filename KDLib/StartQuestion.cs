using Newtonsoft.Json;

namespace KDLib
{
    public class StartQuestion : Question
    {
        #region PrivateMembers
        private SubjectInfo _Subject;
        private string _Type;
        #endregion

        #region PublicPropeties
        public SubjectInfo Subject
        {
            get => _Subject;
            set => Set(ref _Subject, (SubjectInfo)value);
        }
        public string Type
        {
            get => _Type;
            set => Set(ref _Type, (value == "vid" || value == "img") ? value : "txt");
        }
        #endregion

        private SubjectInfo SubjectValue(string value)
        {
            if (!KDConvert.StringToSubject.ContainsKey(value)) return SubjectInfo.Unknown;
            else return KDConvert.StringToSubject[value];
        }

        public StartQuestion(string subject_name, string type, string content, string answer, int id)
            : base(id, content, answer)
        {
            Subject = SubjectValue(subject_name);
            Type = type;
        }

        [JsonConstructor]
        public StartQuestion(int id, SubjectInfo subject, string type, string content, string answer)
    : base(id, content, answer)
        {
            Subject = subject;
            Type = type;
        }

        public void GetValueFrom(StartQuestion sq)
        {
            Subject = sq.Subject;
            Content = sq.Content;
            Answer = sq.Answer;
        }

        public static int SubjectComparer(StartQuestion a, StartQuestion b)
        {
            if (a.Subject < b.Subject) return -1;
            if (a.Subject > b.Subject) return 1;
            return 0;
        }
    }
}
