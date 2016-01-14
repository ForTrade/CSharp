using System;
namespace SmartQuant
{
	public interface IDataProvider : IProvider
	{
		void Subscribe(Instrument instrument);
		void Subscribe(InstrumentList instrument);
		void Unsubscribe(Instrument instrument);
		void Unsubscribe(InstrumentList instrument);
	}
}
