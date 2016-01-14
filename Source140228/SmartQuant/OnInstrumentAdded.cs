using System;
namespace SmartQuant
{
	public class OnInstrumentAdded : Event
	{
		internal Instrument instrument;
		public override byte TypeId
		{
			get
			{
				return 100;
			}
		}
		public OnInstrumentAdded(Instrument instrument)
		{
			this.instrument = instrument;
		}
	}
}
