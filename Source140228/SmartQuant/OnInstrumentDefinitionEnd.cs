using System;
namespace SmartQuant
{
	public class OnInstrumentDefinitionEnd : Event
	{
		internal InstrumentDefinitionEnd end;
		public override byte TypeId
		{
			get
			{
				return 124;
			}
		}
		public OnInstrumentDefinitionEnd(InstrumentDefinitionEnd end)
		{
			this.end = end;
		}
	}
}
