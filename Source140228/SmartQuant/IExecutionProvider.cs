using System;
namespace SmartQuant
{
	public interface IExecutionProvider : IProvider
	{
		void Send(ExecutionCommand command);
	}
}
