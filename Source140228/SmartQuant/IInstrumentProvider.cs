using System;
namespace SmartQuant
{
	public interface IInstrumentProvider : IProvider
	{
		void Send(InstrumentDefinitionRequest request);
		void Cancel(string requestId);
	}
}
