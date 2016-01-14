using System;
namespace SmartQuant
{
	public class InstrumentDefinitionEventArgs : EventArgs
	{
		public InstrumentDefinition Definition
		{
			get;
			private set;
		}
		public InstrumentDefinitionEventArgs(InstrumentDefinition definition)
		{
			this.Definition = definition;
		}
	}
}
