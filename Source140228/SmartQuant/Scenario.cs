using System;
using System.Threading;
namespace SmartQuant
{
	public class Scenario
	{
		protected Framework framework;
		protected Strategy strategy;
		public Clock Clock
		{
			get
			{
				return this.framework.clock;
			}
		}
		public InstrumentManager InstrumentManager
		{
			get
			{
				return this.framework.instrumentManager;
			}
		}
		public ProviderManager ProviderManager
		{
			get
			{
				return this.framework.providerManager;
			}
		}
		public OrderManager OrderManager
		{
			get
			{
				return this.framework.orderManager;
			}
		}
		public IDataSimulator DataSimulator
		{
			get
			{
				return this.framework.providerManager.dataSimulator;
			}
		}
		public IExecutionSimulator ExecutionSimulator
		{
			get
			{
				return this.framework.providerManager.executionSimulator;
			}
		}
		public BarFactory BarFactory
		{
			get
			{
				return this.framework.eventManager.factory;
			}
		}
		public EventManager EventManager
		{
			get
			{
				return this.framework.eventManager;
			}
		}
		public GroupManager GroupManager
		{
			get
			{
				return this.framework.groupManager;
			}
		}
		public DataFileManager DataFileManager
		{
			get
			{
				return this.framework.DataFileManager;
			}
		}
		public Scenario(Framework framework)
		{
			this.framework = framework;
		}
		public void StartStrategy(StrategyMode mode)
		{
			Console.WriteLine(DateTime.Now + " Scenario::StartStrategy " + mode);
			this.framework.strategyManager.StartStrategy(this.strategy, mode);
			while (this.strategy.Status != StrategyStatus.Stopped)
			{
				Thread.Sleep(10);
			}
			Console.WriteLine(DateTime.Now + " Scenario::StartStrategy Done");
		}
		public void StartStrategy()
		{
			Console.WriteLine(DateTime.Now + " Scenario::StartStrategy " + this.framework.strategyManager.Mode);
			this.framework.strategyManager.StartStrategy(this.strategy, this.framework.strategyManager.Mode);
			while (this.strategy.Status != StrategyStatus.Stopped)
			{
				Thread.Sleep(10);
			}
			Console.WriteLine(DateTime.Now + " Scenario::StartStrategy Done");
		}
		public void StartStrategy(Strategy strategy)
		{
			Console.WriteLine(DateTime.Now + " Scenario::StartStrategy " + this.framework.strategyManager.Mode);
			this.framework.strategyManager.StartStrategy(strategy);
			while (strategy.Status != StrategyStatus.Stopped)
			{
				Thread.Sleep(10);
			}
			Console.WriteLine(DateTime.Now + " Scenario::StartStrategy Done");
		}
		public void StartBacktest()
		{
			this.StartStrategy(StrategyMode.Backtest);
		}
		public void StartPaper()
		{
			this.StartStrategy(StrategyMode.Paper);
		}
		public void StartLive()
		{
			this.StartStrategy(StrategyMode.Live);
		}
		public virtual void Run()
		{
		}
	}
}
