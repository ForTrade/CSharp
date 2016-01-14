using System;
using System.Collections.Generic;
namespace SmartQuant
{
	public class MetaStrategy : Strategy
	{
		private IdArray<List<Strategy>> strategiesByInstrument;
		private IdArray<Strategy> strategyById;
		private IdArray<Strategy> strategyByPortfolioId;
		internal new List<Strategy> strategies;
		public MetaStrategy(Framework framework, string name) : base(framework, name)
		{
			this.strategiesByInstrument = new IdArray<List<Strategy>>(1000);
			this.strategyById = new IdArray<Strategy>(1000);
			this.strategyByPortfolioId = new IdArray<Strategy>(1000);
			this.strategies = new List<Strategy>();
		}
		public void Add(Strategy strategy)
		{
			this.strategies.Add(strategy);
			strategy.portfolio.Parent = this.portfolio;
			foreach (Instrument current in strategy.Instruments)
			{
				List<Strategy> list;
				if (this.strategiesByInstrument[current.Id] == null)
				{
					list = new List<Strategy>();
					this.strategiesByInstrument[current.Id] = list;
				}
				else
				{
					list = this.strategiesByInstrument[current.Id];
				}
				list.Add(strategy);
				if (!base.Instruments.Contains(current))
				{
					base.Instruments.Add(current);
				}
			}
		}
		internal override void OnBar_(Bar bar)
		{
			foreach (Strategy current in this.strategiesByInstrument[bar.instrumentId])
			{
				current.OnBar_(bar);
			}
			base.OnBar_(bar);
		}
		internal override void OnTrade_(Trade trade)
		{
			foreach (Strategy current in this.strategiesByInstrument[trade.instrumentId])
			{
				current.OnTrade_(trade);
			}
			base.OnTrade_(trade);
		}
		internal override void OnBid_(Bid bid)
		{
			foreach (Strategy current in this.strategiesByInstrument[bid.instrumentId])
			{
				current.OnBid_(bid);
			}
			base.OnBid_(bid);
		}
		internal override void OnAsk_(Ask ask)
		{
			foreach (Strategy current in this.strategiesByInstrument[ask.instrumentId])
			{
				current.OnAsk_(ask);
			}
			base.OnAsk_(ask);
		}
		internal override void OnExecutionReport_(ExecutionReport report)
		{
			this.strategyById[report.Order.strategyId].OnExecutionReport_(report);
			base.OnExecutionReport_(report);
		}
		internal override void OnOrderCancelled_(Order order)
		{
			this.strategyById[order.strategyId].OnOrderCancelled_(order);
			base.OnOrderCancelled_(order);
		}
		internal override void OnOrderDone_(Order order)
		{
			this.strategyById[order.strategyId].OnOrderDone_(order);
			base.OnOrderDone_(order);
		}
		internal override void OnOrderFilled_(Order order)
		{
			this.strategyById[order.strategyId].OnOrderFilled_(order);
			base.OnOrderFilled_(order);
		}
		internal override void OnOrderPartiallyFilled_(Order order)
		{
			this.strategyById[order.strategyId].OnOrderPartiallyFilled_(order);
			base.OnOrderPartiallyFilled_(order);
		}
		internal override void OnOrderReplaced_(Order order)
		{
			this.strategyById[order.strategyId].OnOrderReplaced_(order);
			base.OnOrderReplaced_(order);
		}
		internal override void OnOrderStatusChanged_(Order order)
		{
			this.strategyById[order.strategyId].OnOrderStatusChanged_(order);
			base.OnOrderStatusChanged_(order);
		}
		internal override void OnPositionChanged_(Position position)
		{
			if (position.portfolio == this.portfolio)
			{
				this.OnPositionChanged(position);
				return;
			}
			foreach (Strategy current in this.strategies)
			{
				current.OnPositionChanged_(position);
			}
		}
		internal override void OnPositionOpened_(Position position)
		{
			if (position.portfolio == this.portfolio)
			{
				this.OnPositionOpened(position);
				return;
			}
			foreach (Strategy current in this.strategies)
			{
				current.OnPositionOpened_(position);
			}
		}
		internal override void OnPositionClosed_(Position position)
		{
			if (position.portfolio == this.portfolio)
			{
				this.OnPositionClosed(position);
				return;
			}
			foreach (Strategy current in this.strategies)
			{
				current.OnPositionClosed_(position);
			}
		}
		internal override void OnFill_(OnFill fill)
		{
			this.strategyById[fill.fill.order.strategyId].OnFill_(fill);
			base.OnFill_(fill);
		}
		internal override void OnProviderConnected_(Provider provider)
		{
			foreach (Strategy current in this.strategies)
			{
				current.OnProviderConnected_(provider);
			}
			base.OnProviderConnected_(provider);
		}
		internal override void OnProviderDisconnected_(Provider provider)
		{
			foreach (Strategy current in this.strategies)
			{
				current.OnProviderDisconnected_(provider);
			}
			base.OnProviderDisconnected_(provider);
		}
		internal override void OnStrategyStart_()
		{
			foreach (Strategy current in this.strategies)
			{
				this.strategyById[(int)current.Id] = current;
				this.strategyByPortfolioId[current.Portfolio.id] = current;
				current.OnStrategyStart_();
			}
			base.OnStrategyStart_();
		}
		internal override void OnStrategyStop_()
		{
			foreach (Strategy current in this.strategies)
			{
				current.OnStrategyStop_();
			}
			base.OnStrategyStop_();
		}
	}
}
