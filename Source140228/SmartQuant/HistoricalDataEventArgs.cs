using System;
namespace SmartQuant
{
	public class HistoricalDataEventArgs : EventArgs
	{
		public HistoricalData Data
		{
			get;
			private set;
		}
		public HistoricalDataEventArgs(HistoricalData data)
		{
			this.Data = data;
		}
	}
}
