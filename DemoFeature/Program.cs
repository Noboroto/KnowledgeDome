using KDLib;

using System;
using System.IO;
using System.Windows;

namespace DemoFeature
{
    class Program
    {
        static void Main(string[] args)
        {
            MatchInfo custom = new MatchInfo("Test Match");
            custom.Players.Add(new Player(0, "Gia Linh"));
            custom.Players.Add(new Player(1, "Quang Phú"));
            custom.Players.Add(new Player(2, "Gia Lạc"));
            custom.Players.Add(new Player(3, "Gia Kiệt"));

            Data.Initialize();
            Data.MatchInfos.Add(custom);

            for (int i = 0; i <= 12; ++i)
			{
                Data.StartQuestions.Add(new StartQuestion(i, (SubjectInfo)i,
                    "Hiện tượng nào là hiện tượng cảm ứng điện từ xảy ra trong một mạch điện do chính sự biến đổi của dòng điện trong mạch đó gây ra?",
                    "Hiện tượng tự cảm", AttachmentType.None
                    ));
			}

            Data.Obstacles.Add(new Obstacle());
            string obstacle = "Olympia Club";
            Data.Obstacles[0].obstacleQuestion = new ObstacleQuestion(11, obstacle.Trim().Length, "png", obstacle);
            for (int i = 12; i <= 16; ++i)
			{
                Data.Obstacles[0].RowList.Add(new ObstacleRowQuestion(i, 11, 5, "asdas", "abcde"));
			}
            for (int i = 17; i <= 17 + 3; ++i)
			{
                Data.AccelerationQuestions.Add(new AccelerationQuestion(i, "png", "abc", "def"));
			}
            for (int i = 21; i <= 33; ++i)
			{
                Data.FinishQuestions.Add(new FinishQuestion(i, (i % 3 + 1) * 10, "qwe", "xyz"));
			}
            for (int i = 34; i <= 36; ++i)
            {
                Data.ExtraQuestions.Add(new ExtraQuestion(i, "qwe", "xyz"));
            }

            Data.MatchInfos.WriteToFile();
            Data.StartQuestions.WriteToFile();
            Data.Obstacles.WriteToFile();
            Data.AccelerationQuestions.WriteToFile();
            Data.FinishQuestions.WriteToFile();
            Data.ExtraQuestions.WriteToFile();
        }
    }
}
