using System;
using System.Diagnostics;
namespace SmartQuant
{
	public class PerformanceStrategy : InstrumentStrategy
	{
		private Stopwatch watch = new Stopwatch();
		public PerformanceStrategy(Framework framework) : base(framework, "PerformanceStrategy")
		{
		}
		protected internal override void OnStrategyStart()
		{
			Trade data = new Trade();
			this.watch.Start();
			(base.DataProvider as PerformanceProvider).EmitData(data);
		}
		protected internal override void OnTrade(Instrument instrument, Trade trade)
		{
			this.watch.Stop();
		}
	}
}
