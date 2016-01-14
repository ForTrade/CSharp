using System;
namespace SmartQuant
{
	public class OnExecutionCommand : Event
	{
		internal ExecutionCommand command;
		public override byte TypeId
		{
			get
			{
				return 114;
			}
		}
		public OnExecutionCommand(ExecutionCommand command)
		{
			this.command = command;
		}
	}
}
