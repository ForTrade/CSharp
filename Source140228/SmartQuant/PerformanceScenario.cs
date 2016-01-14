using System;
namespace SmartQuant
{
	public class PerformanceScenario : Scenario
	{
		public PerformanceScenario(Framework framework) : base(framework)
		{
		}
		public override void Run()
		{
			this.strategy = new PerformanceStrategy(this.framework);
			Provider provider = new PerformanceProvider(this.framework);
			this.strategy.DataProvider = (provider as IDataProvider);
			this.strategy.ExecutionProvider = (provider as IExecutionProvider);
			base.StartStrategy(StrategyMode.Live);
		}
	}
}
