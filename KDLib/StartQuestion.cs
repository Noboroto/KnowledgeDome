using Newtonsoft.Json;

namespace KDLib
{
	public class StartQuestion : Question
	{
		#region PrivateMembers
		private SubjectInfo _Subject;
		private AttachmentType _AttachmentInfo;
		#endregion

		#region PublicPropeties
		public SubjectInfo Subject
		{
			get => _Subject;
			set => Set(ref _Subject, (SubjectInfo)value);
		}
		public AttachmentType AttachmentInfo
		{
			get => _AttachmentInfo;
			set => Set(ref _AttachmentInfo, (AttachmentType)value);
		}
		#endregion

		private SubjectInfo SubjectValue(string value)
		{
			if (!KDConvert.StringToSubject.ContainsKey(value)) return SubjectInfo.Unknown;
			else return KDConvert.StringToSubject[value];
		}

		private AttachmentType AttachmentTypeValue(string value)
		{
			if (!KDConvert.StringToAttachmentType.ContainsKey(value)) return AttachmentType.None;
			else return KDConvert.StringToAttachmentType[value];
		}

		public StartQuestion() : base(0, "", "")
		{

		}

		public StartQuestion(string subject_name, string content, string answer, int id, string attachmenttype = "none")
			: base(id, content, answer)
		{
			Subject = SubjectValue(subject_name);
			AttachmentInfo = AttachmentTypeValue(attachmenttype);
		}

		[JsonConstructor]
		public StartQuestion(int id, SubjectInfo subject, string content, string answer, AttachmentType type)
	: base(id, content, answer)
		{
			Subject = subject;
			AttachmentInfo = type;
		}

		public static int SubjectComparer(StartQuestion a, StartQuestion b)
		{
			if (a.Subject < b.Subject) return -1;
			if (a.Subject > b.Subject) return 1;
			return 0;
		}

		public static StartQuestion FromJson(string source)
		{
			return JsonConvert.DeserializeObject<StartQuestion>(source);
		}
	}
}
