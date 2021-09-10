namespace KDLib.MessageForUI
{
	public class ChangeState
	{
		public ProgramState State { get; set; }
		public ChangeState(ProgramState state)
		{
			State = state;
		}
	}
}
