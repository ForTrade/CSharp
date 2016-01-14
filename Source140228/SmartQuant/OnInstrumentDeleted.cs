using System;
namespace SmartQuant
{
	public class OnInstrumentDeleted : Event
	{
		internal Instrument instrument;
		public override byte TypeId
		{
			get
			{
				return 101;
			}
		}
		public OnInstrumentDeleted(Instrument instrument)
		{
			this.instrument = instrument;
		}
	}
}
