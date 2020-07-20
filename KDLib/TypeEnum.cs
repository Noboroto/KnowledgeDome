using System.ComponentModel;

namespace KDLib
{
    public enum SubjectInfo
    {
        Geography,
        General,
        Chemistry,
        History,
        Other,
        Arts,
        Biology,
        Sport,
        English,
        Math,
        Literature,
        Physics,
        Unknown
    }
    public enum MachineType
    {
        Server,
        [Description("Thí sinh 1")]
        Player_1,
        [Description("Thí sinh 2")]
        Player_2,
        [Description("Thí sinh 3")]
        Player_3,
        [Description("Thí sinh 4")]
        Player_4,
        MC,
        [Description("Khán giả")]
        Viewer
    }
    public enum CommandType
    {
        AskForConnect,
        Forcusing,
        LostForcus,
        Discconect,
        Show,
        Hide,
        Answer,
        IsConnected,
        Question,
        Start,
        Right,
        ScoreEdited,
        Wrong,
        ShowImage,
        HideImage,
        NavigationTo,
        OK,
    }
}
