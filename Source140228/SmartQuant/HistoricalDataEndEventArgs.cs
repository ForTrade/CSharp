using System;
namespace SmartQuant
{
	public class HistoricalDataEndEventArgs : EventArgs
	{
		public HistoricalDataEnd End
		{
			get;
			private set;
		}
		public HistoricalDataEndEventArgs(HistoricalDataEnd end)
		{
			this.End = end;
		}
	}
}
