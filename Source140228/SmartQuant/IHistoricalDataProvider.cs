using System;
namespace SmartQuant
{
	public interface IHistoricalDataProvider : IProvider
	{
		void Send(HistoricalDataRequest request);
		void Cancel(string requestId);
	}
}
