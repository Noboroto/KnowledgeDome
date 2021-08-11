using System.Collections.Generic;
using System.Text;

namespace KDLib
{
	public static class KDConvert
	{
		#region PrivateMembers
		#endregion

		#region PublicMembers
		public static UTF8Encoding UTF8Encoder = new UTF8Encoding();
		public static Dictionary<string, SubjectInfo> StringToSubject = new Dictionary<string, SubjectInfo>()
		{
			["Địa lý"] = SubjectInfo.Geography,
			["Địa lí"] = SubjectInfo.Geography,
			["Hiểu biết chung"] = SubjectInfo.General,
			["Hoá học"] = SubjectInfo.Chemistry,
			["Hóa học"] = SubjectInfo.Chemistry,
			["Lịch sử"] = SubjectInfo.History,
			["Lĩnh vực khác"] = SubjectInfo.Other,
			["Nghệ thuật"] = SubjectInfo.Arts,
			["Sinh học"] = SubjectInfo.Biology,
			["Thể thao"] = SubjectInfo.Sport,
			["Tiếng Anh"] = SubjectInfo.English,
			["Toán học"] = SubjectInfo.Math,
			["Văn học"] = SubjectInfo.Literature,
			["Vật lý"] = SubjectInfo.Physics,
			["Vật lí"] = SubjectInfo.Physics,
		};
		public static Dictionary<SubjectInfo, string> SubjectToString = new Dictionary<SubjectInfo, string>()
		{
			[SubjectInfo.Geography] = "Địa lý",
			[SubjectInfo.General] = "Hiểu biết chung",
			[SubjectInfo.Chemistry] = "Hoá học",
			[SubjectInfo.History] = "Lịch sử",
			[SubjectInfo.Other] = "Lĩnh vực khác",
			[SubjectInfo.Arts] = "Nghệ thuật",
			[SubjectInfo.Biology] = "Sinh học",
			[SubjectInfo.Sport] = "Thể thao",
			[SubjectInfo.English] = "Tiếng Anh",
			[SubjectInfo.Math] = "Toán học",
			[SubjectInfo.Literature] = "Văn học",
			[SubjectInfo.Physics] = "Vật lý",
			[SubjectInfo.Unknown] = "Không xác định"
		};
		#endregion
	}
}
