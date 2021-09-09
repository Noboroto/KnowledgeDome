using System.ComponentModel;

namespace KDLib
{
	public enum AttachmentType
	{
		None,
		Video,
		Sound,
		Image
	}
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
	public enum Machine
	{
		Server,
		[Description("Thí sinh")]
		Player,
		MC,
		[Description("Khán giả")]
		Viewer,
		None
	}

	public enum LogType
	{
		Server,
		Player,
		MC,
		Viewer,
		Warn,
		Error
	}

	public enum ProgramState
	{
		Idling,
		Pending,
		Playing,
		Ended
	}

	public enum CommandType
	{
		ConfirmIP,
		Forcusing,
		LostForcus,
		Discconect,
		IsConnected,
		AskForConnect,
		RefuseConnect,
		ClientList,
		AccpetConnect,
		Right,
		Wrong,
		NextQuestAt,
		NavigateToRound,
		MCToServer,
		ServerToMC,
		MCToMC,
		ChangeMatchToID,
		EditScore,
		ChoosePlayer,
		StopEmergency,
		StartTimmer
	}
}
