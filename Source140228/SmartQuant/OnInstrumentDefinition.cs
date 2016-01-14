using System;
namespace SmartQuant
{
	public class OnInstrumentDefinition : Event
	{
		internal InstrumentDefinition definition;
		public override byte TypeId
		{
			get
			{
				return 123;
			}
		}
		public OnInstrumentDefinition(InstrumentDefinition definition)
		{
			this.definition = definition;
		}
	}
}
