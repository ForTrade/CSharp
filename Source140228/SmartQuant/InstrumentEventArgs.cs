using System;
namespace SmartQuant
{
	public class InstrumentEventArgs : EventArgs
	{
		public Instrument Instrument
		{
			get;
			private set;
		}
		public InstrumentEventArgs(Instrument instrument)
		{
			this.Instrument = instrument;
		}
	}
}
