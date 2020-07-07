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
				_Subject = value;
				OnPropertyChanged("Subject");
			}
		}

		private SubjectInfo SubjectValue (string value)
		{
			switch (value)
			{
				case "Địa lý":
					return SubjectInfo.Geography;
				case "Hiểu biết chung":
					return SubjectInfo.BaseKnowledge;
				case "Hoá học":
					return SubjectInfo.Chemistry;
				case "Lịch sử":
					return SubjectInfo.History;
				case "Lĩnh vực khác":
					return SubjectInfo.Other;
				case "Nghệ thuật":
					return SubjectInfo.Art;
				case "Sinh học":
					return SubjectInfo.Biology;
				case "Thể thao":
					return SubjectInfo.Sport;
				case "Tiếng Anh":
					return SubjectInfo.English;
				case "Toán học":
					return SubjectInfo.Math;
				case "Văn học":
					return SubjectInfo.Literature;
				case "Vật lý":
					return SubjectInfo.Physics;
				default:
					return SubjectInfo.Unknown;
			}
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
