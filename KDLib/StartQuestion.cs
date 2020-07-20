using Newtonsoft.Json;

namespace KDLib
{
	public class StartQuestion : Question
	{
		private SubjectInfo _Subject;

		public SubjectInfo Subject
		{
			get
			{
				return _Subject;
			}
			set
			{
				_Subject = (SubjectInfo)value;
				OnPropertyChanged("Subject");
			}
		}

		private SubjectInfo SubjectValue (string value)
		{
			if (!KDConvert.StringToSubject.ContainsKey(value)) return SubjectInfo.Unknown;
			else return KDConvert.StringToSubject[value];
		}

		public StartQuestion(string subject_name, string content, string answer)
			: this(subject_name, content, answer, Data.GenerateID())
		{
		}

		public StartQuestion(string subject_name, string content, string answer, int id)
			: base(id, content, answer)
		{
			Subject = SubjectValue (subject_name);
		}

		[JsonConstructor]
		public StartQuestion(SubjectInfo subject, string content, string answer, int id)
	: base(id, content, answer)
		{
			Subject = subject;
		}

		public void GetValueFrom(StartQuestion sq)
		{
			Subject = sq.Subject;
			base.Content = sq.Content;
			base.Answer = sq.Answer;
		}

		public static int SubjectComparer(StartQuestion a, StartQuestion b)
		{
			if (a.Subject < b.Subject) return -1;
			if (a.Subject > b.Subject) return 1;
			return 0;
		}
	}
}
