using System;
using System.Collections.Generic;
namespace SmartQuant
{
	public class StrategyManager
	{
		private Framework framework;
		private StrategyMode mode = StrategyMode.Backtest;
		private StrategyStatus status = StrategyStatus.Stopped;
		private Dictionary<IDataProvider, InstrumentList> requests = new Dictionary<IDataProvider, InstrumentList>();
		private byte next_id;
		private Strategy strategy;
		internal Global global = new Global();
		public Global Global
		{
			get
			{
				return this.global;
			}
		}
		public StrategyMode Mode
		{
			get
			{
				return this.mode;
			}
			set
			{
				if (this.mode != value)
				{
					this.mode = value;
					switch (this.mode)
					{
					case StrategyMode.Backtest:
						this.framework.Mode = FrameworkMode.Simulation;
						return;
					case StrategyMode.Paper:
						this.framework.Mode = FrameworkMode.Realtime;
						return;
					case StrategyMode.Live:
						this.framework.Mode = FrameworkMode.Realtime;
						break;
					default:
						return;
					}
				}
			}
		}
		public StrategyManager(Framework framework)
		{
			this.framework = framework;
			this.Clear();
		}
		public void RegisterMarketDataRequest(IDataProvider dataProvider, InstrumentList instrumentList)
		{
			InstrumentList instrumentList2 = new InstrumentList();
			InstrumentList instrumentList3 = null;
			if (!this.requests.TryGetValue(dataProvider, out instrumentList3))
			{
				instrumentList3 = new InstrumentList();
				this.requests[dataProvider] = instrumentList3;
			}
			foreach (Instrument current in instrumentList)
			{
				if (!instrumentList3.Contains(current.id))
				{
					instrumentList3.Add(current);
					instrumentList2.Add(current);
				}
			}
			if (this.status == StrategyStatus.Running && instrumentList2.Count > 0 && this.framework.SubscriptionManager != null)
			{
				this.framework.SubscriptionManager.Subscribe(dataProvider, instrumentList2);
			}
		}
		internal void UnregisterMarketDataRequest(IDataProvider dataProvider, InstrumentList instrumentList)
		{
			if (this.status == StrategyStatus.Running && instrumentList.Count > 0 && this.framework.SubscriptionManager != null)
			{
				this.framework.SubscriptionManager.Unsubscribe(dataProvider, instrumentList);
			}
		}
		public void StartStrategy(Strategy strategy)
		{
			this.StartStrategy(strategy, this.mode);
		}
		public void StartStrategy(Strategy strategy, StrategyMode mode)
		{
			this.strategy = strategy;
			if (mode == StrategyMode.Backtest)
			{
				this.framework.Mode = FrameworkMode.Simulation;
			}
			else
			{
				this.framework.Mode = FrameworkMode.Realtime;
			}
			if (this.framework.eventManager.status != EventManagerStatus.Running)
			{
				this.framework.eventManager.Start();
			}
			StrategyStatusInfo strategyStatusInfo = new StrategyStatusInfo(this.framework.clock.DateTime, StrategyStatusType.Started);
			strategyStatusInfo.Solution = ((strategy.Name == null) ? "Solution" : strategy.Name);
			strategyStatusInfo.Mode = mode.ToString();
			this.framework.eventServer.OnLog(new GroupEvent(strategyStatusInfo, null));
			strategy.OnStrategyStart_();
			if (!this.framework.IsExternalDataQueue)
			{
				Dictionary<IDataProvider, InstrumentList> dictionary = new Dictionary<IDataProvider, InstrumentList>();
				while (this.requests.Count != 0)
				{
					Dictionary<IDataProvider, InstrumentList> dictionary2 = new Dictionary<IDataProvider, InstrumentList>(this.requests);
					this.requests.Clear();
					foreach (KeyValuePair<IDataProvider, InstrumentList> current in new Dictionary<IDataProvider, InstrumentList>(dictionary2))
					{
						InstrumentList instrumentList = null;
						if (!dictionary.TryGetValue(current.Key, out instrumentList))
						{
							instrumentList = new InstrumentList();
							dictionary[current.Key] = instrumentList;
						}
						InstrumentList instrumentList2 = new InstrumentList();
						foreach (Instrument current2 in current.Value)
						{
							if (!instrumentList.Contains(current2))
							{
								instrumentList.Add(current2);
								instrumentList2.Add(current2);
							}
						}
						if (current.Key is SellSideStrategy && this.framework.SubscriptionManager != null)
						{
							this.framework.SubscriptionManager.Subscribe(current.Key, instrumentList2);
						}
					}
				}
				this.status = StrategyStatus.Running;
				this.requests = dictionary;
				if (this.requests.Count == 0)
				{
					Console.WriteLine(string.Concat(new object[]
					{
						DateTime.Now,
						" StrategyManager::StartStrategy ",
						strategy.Name,
						" has no data requests, stopping..."
					}));
					this.StopStrategy();
					return;
				}
				foreach (KeyValuePair<IDataProvider, InstrumentList> current3 in this.requests)
				{
					if (!(current3.Key is SellSideStrategy) && this.framework.SubscriptionManager != null)
					{
						this.framework.SubscriptionManager.Subscribe(current3.Key, current3.Value);
					}
				}
			}
		}
		private void StopStrategy()
		{
			Console.WriteLine(DateTime.Now + " StrategyManager::StopStrategy " + this.strategy.Name);
			if (!this.framework.IsExternalDataQueue)
			{
				foreach (KeyValuePair<IDataProvider, InstrumentList> current in new Dictionary<IDataProvider, InstrumentList>(this.requests))
				{
					if (this.framework.SubscriptionManager != null)
					{
						this.framework.SubscriptionManager.Unsubscribe(current.Key, current.Value);
					}
				}
			}
			if (this.strategy.status == StrategyStatus.Stopped)
			{
				Console.WriteLine("StrategyManager::Stop Error: Can not stop stopped strategy ( " + this.strategy.Name + ")");
				return;
			}
			this.strategy.OnStrategyStop_();
			if (this.framework.Mode == FrameworkMode.Simulation)
			{
				this.framework.providerManager.dataSimulator.Disconnect();
				this.framework.providerManager.executionSimulator.Disconnect();
			}
			if (this.strategy.DataProvider != null)
			{
				this.strategy.DataProvider.Disconnect();
			}
			if (this.strategy.ExecutionProvider != null)
			{
				this.strategy.ExecutionProvider.Disconnect();
			}
			this.status = StrategyStatus.Stopped;
		}
		internal void OnProviderConnected(Provider provider)
		{
			if (this.strategy != null && this.strategy.status == StrategyStatus.Running)
			{
				this.strategy.OnProviderConnected_(provider);
			}
		}
		internal void OnProviderDisconnected(Provider provider)
		{
			if (this.strategy != null && this.strategy.status == StrategyStatus.Running)
			{
				this.strategy.OnProviderDisconnected_(provider);
			}
		}
		internal void OnBid(Bid bid)
		{
			if (this.strategy != null && this.strategy.status == StrategyStatus.Running)
			{
				this.strategy.OnBid_(bid);
			}
		}
		internal void OnAsk(Ask ask)
		{
			if (this.strategy != null && this.strategy.status == StrategyStatus.Running)
			{
				this.strategy.OnAsk_(ask);
			}
		}
		internal void OnTrade(Trade trade)
		{
			if (this.strategy != null && this.strategy.status == StrategyStatus.Running)
			{
				this.strategy.OnTrade_(trade);
			}
		}
		internal void OnLevel2(Level2Snapshot snapshot)
		{
			if (this.strategy != null && this.strategy.status == StrategyStatus.Running)
			{
				this.strategy.OnLevel2_(snapshot);
			}
		}
		internal void OnLevel2(Level2Update update)
		{
			if (this.strategy != null && this.strategy.status == StrategyStatus.Running)
			{
				this.strategy.OnLevel2_(update);
			}
		}
		internal void OnBar(Bar bar)
		{
			if (this.strategy != null && this.strategy.status == StrategyStatus.Running)
			{
				this.strategy.OnBar_(bar);
			}
		}
		internal void OnNews(News news)
		{
			if (this.strategy != null && this.strategy.status == StrategyStatus.Running)
			{
				this.strategy.OnNews_(news);
			}
		}
		internal void OnFundamental(Fundamental fundamental)
		{
			if (this.strategy != null && this.strategy.status == StrategyStatus.Running)
			{
				this.strategy.OnFundamental_(fundamental);
			}
		}
		internal void OnOrderStatusChanged(Order order)
		{
			if (this.strategy != null && this.strategy.status == StrategyStatus.Running)
			{
				this.strategy.OnOrderStatusChanged_(order);
			}
		}
		internal void OnOrderFilled(Order order)
		{
			if (this.strategy != null && this.strategy.status == StrategyStatus.Running)
			{
				this.strategy.OnOrderFilled_(order);
			}
		}
		internal void OnOrderPartiallyFilled(Order order)
		{
			if (this.strategy != null && this.strategy.status == StrategyStatus.Running)
			{
				this.strategy.OnOrderPartiallyFilled_(order);
			}
		}
		internal void OnOrderCancelled(Order order)
		{
			if (this.strategy != null && this.strategy.status == StrategyStatus.Running)
			{
				this.strategy.OnOrderCancelled_(order);
			}
		}
		internal void OnOrderReplaced(Order order)
		{
			if (this.strategy != null && this.strategy.status == StrategyStatus.Running)
			{
				this.strategy.OnOrderReplaced_(order);
			}
		}
		internal void OnOrderDone(Order order)
		{
			if (this.strategy != null && this.strategy.status == StrategyStatus.Running)
			{
				this.strategy.OnOrderDone_(order);
			}
		}
		internal void OnExecutionReport(ExecutionReport report)
		{
			if (this.strategy != null && this.strategy.status == StrategyStatus.Running)
			{
				this.strategy.OnExecutionReport_(report);
			}
		}
		internal void OnFill(OnFill fill)
		{
			if (this.strategy != null && this.strategy.status == StrategyStatus.Running)
			{
				this.strategy.OnFill_(fill);
			}
		}
		internal void OnPositionOpened(Portfolio portfolio, Position position)
		{
			if (this.strategy != null && this.strategy.status == StrategyStatus.Running)
			{
				this.strategy.OnPositionOpened_(position);
			}
		}
		internal void OnPositionClosed(Portfolio portfolio, Position position)
		{
			if (this.strategy != null && this.strategy.status == StrategyStatus.Running)
			{
				this.strategy.OnPositionClosed_(position);
			}
		}
		internal void OnPositionChanged(Portfolio portfolio, Position position)
		{
			if (this.strategy != null && this.strategy.status == StrategyStatus.Running)
			{
				this.strategy.OnPositionChanged_(position);
			}
		}
		public void Clear()
		{
			this.next_id = 1;
			this.requests.Clear();
			this.global.Clear();
		}
		public void Stop()
		{
			if (this.status != StrategyStatus.Stopped)
			{
				this.status = StrategyStatus.Stopped;
				this.StopStrategy();
			}
			this.Clear();
		}
		public byte GetNextId()
		{
			byte result;
            next_id = (byte)(Convert.ToByte((result = next_id) + 1));
			return result;
		}
	}
}
