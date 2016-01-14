using System;
namespace SmartQuant
{
	public class OnUnsubscribe : Event
	{
		internal InstrumentList instruments;
		internal Instrument instrument;
		public override byte TypeId
		{
			get
			{
				return 204;
			}
		}
		public OnUnsubscribe(InstrumentList instruments)
		{
			this.instruments = instruments;
		}
		public OnUnsubscribe(Instrument instrument)
		{
			this.instrument = instrument;
		}
	}
}
