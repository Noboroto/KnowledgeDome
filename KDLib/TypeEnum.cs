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
        [Description("Thí sinh")]
        Player,
        MC,
        [Description("Khán giả")]
        Viewer,
        None
    }
    public enum CommandType
    {
        Forcusing,
        LostForcus,
        Discconect,
        IsConnected,
        AskForConnect,
        RefuseConnect,
        ClientList,
        AccpetConnect
    }
}
