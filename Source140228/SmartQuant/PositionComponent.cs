using System;
namespace SmartQuant
{
	public class PositionComponent : StrategyComponent
	{
		public virtual void OnSignal(Signal signal)
		{
		}
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
		public virtual void OnPositionOpened(Position position)
		{
		}
		public virtual void OnPositionClosed(Position position)
		{
		}
		public virtual void OnPositionChanged(Position position)
		{
		}
		public virtual void OnStopExecuted(Stop stop)
		{
		}
		public virtual void OnStopCancelled(Stop stop)
		{
		}
	}
}
