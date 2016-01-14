using System;
namespace SmartQuant
{
	public class ExecutionComponent : StrategyComponent
	{
		public virtual void OnOrder(Order order)
		{
			order.provider = this.strategy.GetExecutionProvider(base.Instrument);
			order.strategyId = (int)this.strategy.id;
			this.strategy.framework.OrderManager.Send(order);
		}
		public virtual void OnExecutionReport(ExecutionReport report)
		{
		}
		public virtual void OnOrderFilled(Order order)
		{
		}
	}
}
