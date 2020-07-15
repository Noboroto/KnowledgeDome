using Newtonsoft.Json;

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
        Player_1,
        Player_2,
        Player_3,
        Player_4,
        MC,
        Viewer
    }
    public enum CommandType
    {
        Discconect,
        Show,
        Hide,
        Answer,
        Online,
        Question,
        Start,
        Right,
        ScoreEdited,
        Wrong,
        ShowImage,
        HideImage,
        NavigationTo
    }
}
