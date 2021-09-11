using System;
using System.Collections.Generic;
using System.Text;

namespace KDLib
{
	public static class KDConvert
	{
		#region PublicMembers
		public static UTF8Encoding UTF8Encoder = new UTF8Encoding();

		public static int UrlStringToID(string Uri)
		{
			switch (Uri)
			{
				case @"pack://application:,,,/KDCtrlLib;component/Views/ServerView/MainServerPage.xaml":
					return 0;
				case @"pack://application:,,,/KDCtrlLib;component/Views/ServerView/StartRoundServerView.xaml":
					return 1;
				default:
					return -100;
			}
		}

		public static Uri FindUri(int id)
		{
			string s = @"pack://application:,,,/KDCtrlLib;component/Views/";
			switch (id)
			{
				case -2:
					s += @"ConnectPage.xaml";
					break;
				case -1:
					s += @"RolePage.xaml";
					break;
				case 0:
					switch (Data.ThisMacineType)
					{
						case Machine.Server:
							s += @"ServerView/MainServerPage.xaml";
							break;
						default:
							s += @"MainClientPage.xaml";
							break;
					}
					break;
				case 1:
					switch (Data.ThisMacineType)
					{
						case Machine.Server:
							s += @"ServerView/StartRoundServerView.xaml";
							break;
						case Machine.MC:
							s += @"MCView/StartRoundMCView.xaml";
							break;
						default:
							s += @"StartRoundViewerPlayerPage.xaml";
							break;
					}
					break;
			}
			return new Uri(s);
		}


		public static string BackgroundPlayer (int index)
		{
			switch(index)
			{
				case 0:
					return "#d71b3b";
				case 1:
					return "#e8d71e";
				case 2:
					return "#16acea";
				case 3:
					return "#3a6b35";
				default:
					return "#4203c9";
			}
		}

		public static string ForegroundPlayer(int index)
		{
			switch (index)
			{
				default:
					return "FFFFFF";
				case 1:
				case 2:
					return "000000";
			}
		}

		public static AttachmentType StringToAttachmentType (string type)
		{
			switch (type)
			{
				case "Image":
					return AttachmentType.Image;
				case "Sound":
					return AttachmentType.Sound;
				case "Video":
					return AttachmentType.Video;
				default:
					return AttachmentType.None;
			}
		}

		public static SubjectInfo StringToSubject(string subject)
		{
			switch (subject)
			{
				case "Địa lý":
				case "Địa lí":
					return SubjectInfo.Geography;
				case "Hiểu biết chung":
					return SubjectInfo.General;
				case "Hoá học":
				case "Hóa học":
					return SubjectInfo.Chemistry;
				case "Lịch sử":
					return SubjectInfo.History;
				case "Lĩnh vực khác":
					return SubjectInfo.Other;
				case "Nghệ thuật":
					return SubjectInfo.Arts;
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
				case "Vật lí":
					return SubjectInfo.Physics;
				default:
					return SubjectInfo.Unknown;
			}
		}

		public static string SubjectToString(SubjectInfo subject)
		{
			switch(subject)
			{
				case SubjectInfo.Geography:
					return "Địa lý";
				case SubjectInfo.General:
					return "Hiểu biết chung";
				case SubjectInfo.Chemistry:
					return "Hoá học";
				case SubjectInfo.History:
					return "Lịch sử";
				case SubjectInfo.Other:
					return "Lĩnh vực khác";
				case SubjectInfo.Arts:
					return "Nghệ thuật";
				case SubjectInfo.Biology:
					return "Sinh học";
				case SubjectInfo.Sport:
					return "Thể thao";
				case SubjectInfo.English:
					return "Tiếng Anh";
				case SubjectInfo.Math:
					return "Toán học";
				case SubjectInfo.Literature:
					return "Văn học";
				case SubjectInfo.Physics:
					return "Vật lý";
				default:
					return "Không xác định";
			}
		}
		#endregion
	}
}
