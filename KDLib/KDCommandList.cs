using System;

namespace KDLib
{
	public class KDCommandList
	{
		private Action<KDCommand> Checker;

		public KDCommandList (Action<KDCommand> checker) 
		{
			Checker = checker;
		}

		public void Enqueue(KDCommand command)
		{
			Checker(command);
		}
	}
}
