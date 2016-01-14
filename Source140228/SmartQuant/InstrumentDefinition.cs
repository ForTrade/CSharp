using System;
namespace SmartQuant
{
	public class InstrumentDefinition
	{
		public string RequestId
		{
			get;
			set;
		}
		public byte ProviderId
		{
			get;
			set;
		}
		public int TotalNum
		{
			get;
			set;
		}
		public Instrument[] Instruments
		{
			get;
			set;
		}
	}
}
