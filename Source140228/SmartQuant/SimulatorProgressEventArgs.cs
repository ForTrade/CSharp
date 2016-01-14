using System;
namespace SmartQuant
{
	public class SimulatorProgressEventArgs : EventArgs
	{
		public long Count
		{
			get;
			private set;
		}
		public int Percent
		{
			get;
			private set;
		}
		public SimulatorProgressEventArgs(long count, int percent)
		{
			this.Count = count;
			this.Percent = percent;
		}
	}
}
