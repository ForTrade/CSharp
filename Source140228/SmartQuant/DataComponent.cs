using System;
namespace SmartQuant
{
	public class DataComponent : StrategyComponent
	{
		public virtual void OnBar(Bar bar)
		{
		}
		public virtual void OnTrade(Trade trade)
		{
		}
		public virtual void OnBid(Bid bid)
		{
		}
		public virtual void OnAsk(Ask ask)
		{
		}
	}
}
