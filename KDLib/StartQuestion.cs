using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KDLib
{
	public class StartQuestion : Question
	{
		private SubjectInfo _Subject;

		[JsonProperty("Subject")]
		[JsonConverter(typeof (StringEnumConverter))]
		public SubjectInfo Subject
		{
			get
			{
				return _Subject;
			}
			set
			{
				_Subject = value;
				OnPropertyChanged("Subject");
			}
		}

		private SubjectInfo SubjectValue (string value)
		{
			if (!KDConvert.StringToSubject.ContainsKey(value)) return SubjectInfo.Unknown;
			else return KDConvert.StringToSubject[value];
		}

		public StartQuestion(string subject_name, string content, string answer)
			: this(Data.GenerateID(), subject_name, content, answer)
		{
		}

		public StartQuestion(int id, string subject_name, string content, string answer)
			: base(id, content, answer)
		{
			Subject = SubjectValue (subject_name);
		}

		[JsonConstructor]
		public StartQuestion(int id, SubjectInfo subject, string content, string answer)
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
