using System;
namespace SmartQuant
{
	public class InstrumentDefinitionEndEventArgs : EventArgs
	{
		public InstrumentDefinitionEnd End
		{
			get;
			private set;
		}
		public InstrumentDefinitionEndEventArgs(InstrumentDefinitionEnd end)
		{
			this.End = end;
		}
	}
}
