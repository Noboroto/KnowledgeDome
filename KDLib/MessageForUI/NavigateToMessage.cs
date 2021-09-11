using Newtonsoft.Json;

namespace KDLib.MessageForUI
{
	public class NavigateToMessage
	{
		public int ID { get; private set; }
		public ProgramState State { get; private set; }
		public NavigateToMessage() : this (Data.CurrentRound, Data.Status)
		{
		}
		public NavigateToMessage(int RoundID) : this (RoundID, Data.Status)
		{
		}
		public NavigateToMessage(ProgramState state) : this(Data.CurrentRound, state)
		{
		}
		[JsonConstructor]
		public NavigateToMessage(int id, int state)
		{
			ID = id;
			State = (ProgramState)state;
		}
		public NavigateToMessage(int RoundID, ProgramState state)
		{
			ID = RoundID;
			State = state;
		}
	}
}
