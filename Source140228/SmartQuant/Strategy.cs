using System;
using System.Collections.Generic;
namespace SmartQuant
{
	public class Strategy
	{
		private IdArray<LinkedList<Strategy>> strategiesByInstrument;
		private IdArray<Strategy> strategyByOrderId;
		protected LinkedList<Strategy> strategies;
		private IdArray<int> instrumentCountTable;
		private Strategy parent;
		protected bool raiseEvents = true;
		protected internal Framework framework;
		internal byte id;
		internal StrategyStatus status;
		internal Portfolio portfolio;
		internal InstrumentList instruments;
		internal IDataProvider dataProvider;
		internal IExecutionProvider executionProvider;
		private TickSeries bids;
		private TickSeries asks;
		private TickSeries trades;
		internal BarSeries bars;
		internal TimeSeries equity;
		internal List<Stop> stops;
		internal IdArray<List<Stop>> stopsByInstrument;
		public byte Id
		{
			get
			{
				return this.id;
			}
		}
		public string Name
		{
			get;
			private set;
		}
		public StrategyMode Mode
		{
			get
			{
				return this.framework.strategyManager.Mode;
			}
		}
		public StrategyStatus Status
		{
			get
			{
				return this.status;
			}
		}
		public Portfolio Portfolio
		{
			get
			{
				return this.portfolio;
			}
		}
		public InstrumentList Instruments
		{
			get
			{
				return this.instruments;
			}
		}
		public BarSeries Bars
		{
			get
			{
				return this.bars;
			}
		}
		public TimeSeries Equity
		{
			get
			{
				return this.equity;
			}
		}
		public TickSeries Bids
		{
			get
			{
				return this.bids;
			}
		}
		public TickSeries Asks
		{
			get
			{
				return this.asks;
			}
		}
		public IDataProvider DataProvider
		{
			get
			{
				return this.dataProvider;
			}
			set
			{
				this.dataProvider = value;
			}
		}
		public IExecutionProvider ExecutionProvider
		{
			get
			{
				return this.executionProvider;
			}
			set
			{
				this.executionProvider = value;
			}
		}
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
		public DataManager DataManager
		{
			get
			{
				return this.framework.dataManager;
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
		public Global Global
		{
			get
			{
				return this.framework.strategyManager.global;
			}
		}
		public Strategy(Framework framework, string name)
		{
			this.framework = framework;
			this.Name = name;
			this.strategiesByInstrument = new IdArray<LinkedList<Strategy>>(1000);
			this.strategyByOrderId = new IdArray<Strategy>(1000);
			this.strategies = new LinkedList<Strategy>();
			this.status = StrategyStatus.Stopped;
			this.instruments = new InstrumentList();
			this.instrumentCountTable = new IdArray<int>(1000);
			if (framework != null)
			{
				this.portfolio = new Portfolio(framework, this.Name);
				framework.PortfolioManager.Add(this.portfolio);
			}
			this.bars = new BarSeries("", "");
			this.equity = new TimeSeries();
			this.bids = new TickSeries("");
			this.asks = new TickSeries("");
			this.stops = new List<Stop>();
			this.stopsByInstrument = new IdArray<List<Stop>>(1000);
		}
		public void AddInstruments(InstrumentList instruments)
		{
			foreach (Instrument current in instruments)
			{
				this.AddInstrument(current);
			}
		}
		public void AddInstruments(string[] symbols)
		{
			for (int i = 0; i < symbols.Length; i++)
			{
				string symbol = symbols[i];
				this.AddInstrument(this.framework.InstrumentManager.Get(symbol));
			}
		}
		public void AddInstrument(Instrument instrument)
		{
			this.AddInstrument_(instrument);
			if (this.status == StrategyStatus.Running)
			{
				this.RegisterInstrument(instrument);
			}
		}
		private void RegisterInstrument(Instrument instrument)
		{
			InstrumentList instrumentList = new InstrumentList();
			instrumentList.Add(instrument);
			IDataProvider dataProvider = this.GetDataProvider(this, instrument);
			IExecutionProvider executionProvider = this.GetExecutionProvider(instrument);
			if (dataProvider.Status == ProviderStatus.Disconnected)
			{
				dataProvider.Connect();
			}
			if (executionProvider.Status == ProviderStatus.Disconnected)
			{
				executionProvider.Connect();
			}
			this.framework.strategyManager.RegisterMarketDataRequest(dataProvider, instrumentList);
			if (this.parent != null)
			{
				this.parent.RegisterStrategy(this, instrumentList, (int)this.id);
			}
		}
		internal void AddInstrument_(Instrument instrument)
		{
			if (this.instruments.Contains(instrument))
			{
				Console.WriteLine("Strategy::AddInstrument " + instrument + " is already added");
				return;
			}
			this.instruments.Add(instrument);
		}
		public void AddInstrument(string symbol)
		{
			Instrument instrument = this.framework.instrumentManager.Get(symbol);
			if (instrument == null)
			{
				Console.WriteLine("Strategy::AddInstrument instrument with symbol " + symbol + " doesn't exist in the framework");
				return;
			}
			this.AddInstrument(instrument);
		}
		public void AddInstrument(int id)
		{
			Instrument byId = this.framework.instrumentManager.GetById(id);
			if (byId == null)
			{
				Console.WriteLine("Strategy::AddInstrument instrument with Id " + id + " doesn't exist in the framework");
				return;
			}
			this.AddInstrument(byId);
		}
		public void RemoveInstrument(Instrument instrument)
		{
			this.instruments.Remove(instrument);
			InstrumentList instrumentList = new InstrumentList();
			instrumentList.Add(instrument);
			this.framework.strategyManager.UnregisterMarketDataRequest(this.GetDataProvider(this, instrument), instrumentList);
			if (this.parent != null)
			{
				this.parent.UnregisterStrategy(this, instrumentList, (int)this.id);
			}
		}
		public bool HasPosition(Instrument instrument)
		{
			return this.portfolio.HasPosition(instrument);
		}
		public bool HasPosition(Instrument instrument, PositionSide side, double qty)
		{
			return this.portfolio.HasPosition(instrument, side, qty);
		}
		public bool HasLongPosition(Instrument instrument)
		{
			return this.portfolio.HasLongPosition(instrument);
		}
		public bool HasLongPosition(Instrument instrument, double qty)
		{
			return this.portfolio.HasLongPosition(instrument, qty);
		}
		public bool HasShortPosition(Instrument instrument)
		{
			return this.portfolio.HasShortPosition(instrument);
		}
		public bool HasShortPosition(Instrument instrument, double qty)
		{
			return this.portfolio.HasShortPosition(instrument, qty);
		}
		public void AddReminder(DateTime dateTime, object data = null)
		{
			this.framework.Clock.AddReminder(new ReminderCallback(this.OnReminder_), dateTime, data);
		}
		internal virtual void OnStrategyStart_()
		{
			this.status = StrategyStatus.Running;
			foreach (Instrument current in this.instruments)
			{
				this.RegisterInstrument(current);
			}
			if (this.raiseEvents)
			{
				this.OnStrategyStart();
			}
			for (LinkedListNode<Strategy> linkedListNode = this.strategies.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				this.RegisterStrategy(linkedListNode.Data, linkedListNode.Data.instruments, (int)linkedListNode.Data.id);
				linkedListNode.Data.OnStrategyStart_();
			}
		}
		internal virtual void OnStrategyStop_()
		{
			this.status = StrategyStatus.Stopped;
			if (this.raiseEvents)
			{
				this.OnStrategyStop();
			}
			for (LinkedListNode<Strategy> linkedListNode = this.strategies.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Data.OnStrategyStop_();
			}
		}
		internal virtual void OnReminder_(DateTime dateTime, object data)
		{
			this.OnReminder(dateTime, data);
		}
		internal virtual void OnProviderConnected_(Provider provider)
		{
			if (this.raiseEvents)
			{
				this.OnProviderConnected(provider);
			}
			for (LinkedListNode<Strategy> linkedListNode = this.strategies.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Data.OnProviderConnected_(provider);
			}
		}
		internal virtual void OnProviderDisconnected_(Provider provider)
		{
			if (this.raiseEvents)
			{
				this.OnProviderDisconnected(provider);
			}
			for (LinkedListNode<Strategy> linkedListNode = this.strategies.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Data.OnProviderDisconnected_(provider);
			}
		}
		internal virtual void OnBid_(Bid bid)
		{
			if (this.raiseEvents && this.instruments.Contains(bid.instrumentId))
			{
				this.OnBid(this.framework.InstrumentManager.GetById(bid.instrumentId), bid);
				List<Stop> list = this.stopsByInstrument[bid.instrumentId];
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						list[i].OnBid(bid);
					}
				}
			}
			LinkedList<Strategy> linkedList = this.strategiesByInstrument[bid.instrumentId];
			if (linkedList != null)
			{
				for (LinkedListNode<Strategy> linkedListNode = linkedList.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
				{
					linkedListNode.Data.OnBid_(bid);
				}
			}
		}
		internal virtual void OnAsk_(Ask ask)
		{
			if (this.raiseEvents && this.instruments.Contains(ask.instrumentId))
			{
				this.OnAsk(this.framework.InstrumentManager.GetById(ask.instrumentId), ask);
			}
			List<Stop> list = this.stopsByInstrument[ask.instrumentId];
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					list[i].OnAsk(ask);
				}
			}
			LinkedList<Strategy> linkedList = this.strategiesByInstrument[ask.instrumentId];
			if (linkedList != null)
			{
				for (LinkedListNode<Strategy> linkedListNode = linkedList.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
				{
					linkedListNode.Data.OnAsk_(ask);
				}
			}
		}
		internal virtual void OnTrade_(Trade trade)
		{
			if (this.raiseEvents && this.instruments.Contains(trade.instrumentId))
			{
				this.OnTrade(this.framework.InstrumentManager.GetById(trade.instrumentId), trade);
			}
			List<Stop> list = this.stopsByInstrument[trade.instrumentId];
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					list[i].OnTrade(trade);
				}
			}
			LinkedList<Strategy> linkedList = this.strategiesByInstrument[trade.instrumentId];
			if (linkedList != null)
			{
				for (LinkedListNode<Strategy> linkedListNode = linkedList.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
				{
					linkedListNode.Data.OnTrade_(trade);
				}
			}
		}
		internal virtual void OnLevel2_(Level2Snapshot snapshot)
		{
			if (this.raiseEvents && this.instruments.Contains(snapshot.instrumentId))
			{
				this.OnLevel2(this.framework.InstrumentManager.GetById(snapshot.instrumentId), snapshot);
			}
			LinkedList<Strategy> linkedList = this.strategiesByInstrument[snapshot.instrumentId];
			if (linkedList != null)
			{
				for (LinkedListNode<Strategy> linkedListNode = linkedList.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
				{
					linkedListNode.Data.OnLevel2_(snapshot);
				}
			}
		}
		internal virtual void OnLevel2_(Level2Update update)
		{
			if (this.raiseEvents)
			{
				this.OnLevel2(this.framework.InstrumentManager.GetById(update.instrumentId), update);
			}
			LinkedList<Strategy> linkedList = this.strategiesByInstrument[update.instrumentId];
			if (linkedList != null)
			{
				for (LinkedListNode<Strategy> linkedListNode = linkedList.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
				{
					linkedListNode.Data.OnLevel2_(update);
				}
			}
		}
		internal virtual void OnBar_(Bar bar)
		{
			if (this.raiseEvents && this.instruments.Contains(bar.instrumentId))
			{
				this.OnBar(this.framework.InstrumentManager.GetById(bar.instrumentId), bar);
				List<Stop> list = this.stopsByInstrument[bar.instrumentId];
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						list[i].OnBar(bar);
					}
				}
			}
			LinkedList<Strategy> linkedList = this.strategiesByInstrument[bar.instrumentId];
			if (linkedList != null)
			{
				for (LinkedListNode<Strategy> linkedListNode = linkedList.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
				{
					linkedListNode.Data.OnBar_(bar);
				}
			}
		}
		internal virtual void OnNews_(News news)
		{
			if (this.raiseEvents && this.instruments.Contains(news.instrumentId))
			{
				this.OnNews(this.framework.InstrumentManager.GetById(news.instrumentId), news);
			}
			LinkedList<Strategy> linkedList = this.strategiesByInstrument[news.instrumentId];
			if (linkedList != null)
			{
				for (LinkedListNode<Strategy> linkedListNode = linkedList.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
				{
					linkedListNode.Data.OnNews_(news);
				}
			}
		}
		internal virtual void OnFundamental_(Fundamental fundamental)
		{
			if (this.raiseEvents && this.instruments.Contains(fundamental.instrumentId))
			{
				this.OnFundamental(this.framework.InstrumentManager.GetById(fundamental.instrumentId), fundamental);
			}
			LinkedList<Strategy> linkedList = this.strategiesByInstrument[fundamental.instrumentId];
			if (linkedList != null)
			{
				for (LinkedListNode<Strategy> linkedListNode = linkedList.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
				{
					linkedListNode.Data.OnFundamental_(fundamental);
				}
			}
		}
		internal virtual void OnExecutionReport_(ExecutionReport report)
		{
			if (this.raiseEvents)
			{
				this.OnExecutionReport(report);
			}
			Strategy strategy = this.strategyByOrderId[report.order.strategyId];
			if (strategy != null)
			{
				strategy.OnExecutionReport_(report);
			}
		}
		internal virtual void OnOrderStatusChanged_(Order order)
		{
			if (this.raiseEvents)
			{
				this.OnOrderStatusChanged(order);
			}
			Strategy strategy = this.strategyByOrderId[order.strategyId];
			if (strategy != null)
			{
				strategy.OnOrderStatusChanged_(order);
			}
		}
		internal virtual void OnOrderFilled_(Order order)
		{
			if (this.raiseEvents)
			{
				this.OnOrderFilled(order);
			}
			Strategy strategy = this.strategyByOrderId[order.strategyId];
			if (strategy != null)
			{
				strategy.OnOrderFilled_(order);
			}
		}
		internal virtual void OnOrderPartiallyFilled_(Order order)
		{
			if (this.raiseEvents)
			{
				this.OnOrderPartiallyFilled(order);
			}
			Strategy strategy = this.strategyByOrderId[order.strategyId];
			if (strategy != null)
			{
				strategy.OnOrderPartiallyFilled_(order);
			}
		}
		internal virtual void OnOrderCancelled_(Order order)
		{
			if (this.raiseEvents)
			{
				this.OnOrderCancelled(order);
			}
			Strategy strategy = this.strategyByOrderId[order.strategyId];
			if (strategy != null)
			{
				strategy.OnOrderCancelled_(order);
			}
		}
		internal virtual void OnOrderReplaced_(Order order)
		{
			if (this.raiseEvents)
			{
				this.OnOrderReplaced(order);
			}
			Strategy strategy = this.strategyByOrderId[order.strategyId];
			if (strategy != null)
			{
				strategy.OnOrderReplaced_(order);
			}
		}
		internal virtual void OnOrderDone_(Order order)
		{
			if (this.raiseEvents)
			{
				this.OnOrderDone(order);
			}
			Strategy strategy = this.strategyByOrderId[order.strategyId];
			if (strategy != null)
			{
				strategy.OnOrderDone_(order);
			}
		}
		internal virtual void OnFill_(OnFill fill)
		{
			if (this.raiseEvents && fill.portfolio == this.portfolio)
			{
				this.OnFill(fill.fill);
			}
			for (LinkedListNode<Strategy> linkedListNode = this.strategies.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Data.OnFill_(fill);
			}
		}
		internal virtual void OnPositionOpened_(Position position)
		{
			if (this.raiseEvents && position.portfolio == this.portfolio)
			{
				this.OnPositionOpened(position);
			}
			for (LinkedListNode<Strategy> linkedListNode = this.strategies.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Data.OnPositionOpened_(position);
			}
		}
		internal virtual void OnPositionClosed_(Position position)
		{
			if (this.raiseEvents && position.portfolio == this.portfolio)
			{
				this.OnPositionClosed(position);
				List<Stop> list = this.stopsByInstrument[position.instrument.Id];
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].position == position)
						{
							list[i].Cancel();
						}
					}
				}
			}
			for (LinkedListNode<Strategy> linkedListNode = this.strategies.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Data.OnPositionClosed_(position);
			}
		}
		internal virtual void OnPositionChanged_(Position position)
		{
			if (this.raiseEvents && position.portfolio == this.portfolio)
			{
				this.OnPositionChanged(position);
			}
			for (LinkedListNode<Strategy> linkedListNode = this.strategies.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Data.OnPositionChanged_(position);
			}
		}
		protected internal virtual void OnStopStatusChanged_(Stop stop)
		{
			if (this.raiseEvents)
			{
				switch (stop.status)
				{
				case StopStatus.Executed:
					this.OnStopExecuted(stop);
					break;
				case StopStatus.Canceled:
					this.OnStopCancelled(stop);
					break;
				}
				this.OnStopStatusChanged(stop);
				this.stops.Remove(stop);
				this.stopsByInstrument[stop.instrument.Id].Remove(stop);
			}
		}
		protected internal virtual void OnStrategyStart()
		{
		}
		protected internal virtual void OnStrategyStop()
		{
		}
		protected internal virtual void OnReminder(DateTime dateTime, object data)
		{
		}
		protected internal virtual void OnProviderConnected(Provider provider)
		{
		}
		protected internal virtual void OnProviderDisconnected(Provider provider)
		{
		}
		protected internal virtual void OnBid(Instrument instrument, Bid bid)
		{
		}
		protected internal virtual void OnAsk(Instrument instrument, Ask ask)
		{
		}
		protected internal virtual void OnTrade(Instrument instrument, Trade trade)
		{
		}
		protected internal virtual void OnLevel2(Instrument instrument, Level2Snapshot snapshot)
		{
		}
		protected internal virtual void OnLevel2(Instrument instrument, Level2Update update)
		{
		}
		protected internal virtual void OnBar(Instrument instrument, Bar bar)
		{
		}
		protected internal virtual void OnNews(Instrument instrument, News news)
		{
		}
		protected internal virtual void OnFundamental(Instrument instrument, Fundamental fundamental)
		{
		}
		protected internal virtual void OnExecutionReport(ExecutionReport report)
		{
		}
		protected internal virtual void OnOrderStatusChanged(Order order)
		{
		}
		protected internal virtual void OnOrderFilled(Order order)
		{
		}
		protected internal virtual void OnOrderPartiallyFilled(Order order)
		{
		}
		protected internal virtual void OnOrderCancelled(Order order)
		{
		}
		protected internal virtual void OnOrderReplaced(Order order)
		{
		}
		protected internal virtual void OnOrderDone(Order order)
		{
		}
		protected internal virtual void OnFill(Fill fill)
		{
		}
		protected internal virtual void OnPositionOpened(Position position)
		{
		}
		protected internal virtual void OnPositionClosed(Position position)
		{
		}
		protected internal virtual void OnPositionChanged(Position position)
		{
		}
		protected internal virtual void OnStopExecuted(Stop stop)
		{
		}
		protected internal virtual void OnStopCancelled(Stop stop)
		{
		}
		protected internal virtual void OnStopStatusChanged(Stop stop)
		{
		}
		public void Send(Order order)
		{
			this.framework.OrderManager.Send(order);
		}
		public void Cancel(Order order)
		{
			this.framework.OrderManager.Cancel(order);
		}
		public void Replace(Order order, double price)
		{
			this.framework.OrderManager.Replace(order, price);
		}
		public Order Buy(Instrument instrument, double qty, string text = "")
		{
			Order order = new Order(this.GetExecutionProvider(instrument), this.portfolio, instrument, OrderType.Market, OrderSide.Buy, qty, 0.0, 0.0, TimeInForce.Day, 0, "");
			order.strategyId = (int)this.id;
			order.text = text;
			this.framework.OrderManager.Send(order);
			return order;
		}
		public Order Sell(Instrument instrument, double qty, string text = "")
		{
			Order order = new Order(this.GetExecutionProvider(instrument), this.portfolio, instrument, OrderType.Market, OrderSide.Sell, qty, 0.0, 0.0, TimeInForce.Day, 0, "");
			order.strategyId = (int)this.id;
			order.text = text;
			this.framework.OrderManager.Send(order);
			return order;
		}
		public Order BuyLimit(Instrument instrument, double qty, double price, string text = "")
		{
			Order order = new Order(this.GetExecutionProvider(instrument), this.portfolio, instrument, OrderType.Limit, OrderSide.Buy, qty, 0.0, 0.0, TimeInForce.Day, 0, "");
			order.strategyId = (int)this.id;
			order.text = text;
			order.price = price;
			this.framework.OrderManager.Send(order);
			return order;
		}
		public Order SellLimit(Instrument instrument, double qty, double price, string text = "")
		{
			Order order = new Order(this.GetExecutionProvider(instrument), this.portfolio, instrument, OrderType.Limit, OrderSide.Sell, qty, 0.0, 0.0, TimeInForce.Day, 0, "");
			order.strategyId = (int)this.id;
			order.text = text;
			order.price = price;
			this.framework.OrderManager.Send(order);
			return order;
		}
		public Order BuyStop(Instrument instrument, double qty, double stopPx, string text = "")
		{
			Order order = new Order(this.GetExecutionProvider(instrument), this.portfolio, instrument, OrderType.Stop, OrderSide.Buy, qty, 0.0, 0.0, TimeInForce.Day, 0, "");
			order.strategyId = (int)this.id;
			order.text = text;
			order.stopPx = stopPx;
			this.framework.OrderManager.Send(order);
			return order;
		}
		public Order SellStop(Instrument instrument, double qty, double stopPx, string text = "")
		{
			Order order = new Order(this.GetExecutionProvider(instrument), this.portfolio, instrument, OrderType.Stop, OrderSide.Sell, qty, 0.0, 0.0, TimeInForce.Day, 0, "");
			order.strategyId = (int)this.id;
			order.text = text;
			order.stopPx = stopPx;
			this.framework.OrderManager.Send(order);
			return order;
		}
		public Order BuyStopLimit(Instrument instrument, double qty, double stopPx, double price, string text = "")
		{
			Order order = new Order(this.GetExecutionProvider(instrument), this.portfolio, instrument, OrderType.StopLimit, OrderSide.Buy, qty, 0.0, 0.0, TimeInForce.Day, 0, "");
			order.strategyId = (int)this.id;
			order.text = text;
			order.stopPx = stopPx;
			order.price = price;
			this.framework.OrderManager.Send(order);
			return order;
		}
		public Order SellStopLimit(Instrument instrument, double qty, double stopPx, double price, string text = "")
		{
			Order order = new Order(this.GetExecutionProvider(instrument), this.portfolio, instrument, OrderType.StopLimit, OrderSide.Sell, qty, 0.0, 0.0, TimeInForce.Day, 0, "");
			order.strategyId = (int)this.id;
			order.text = text;
			order.stopPx = stopPx;
			order.price = price;
			this.framework.OrderManager.Send(order);
			return order;
		}
		public Order BuyOrder(Instrument instrument, double qty, string text = "")
		{
			Order order = new Order(this.GetExecutionProvider(instrument), this.portfolio, instrument, OrderType.Market, OrderSide.Buy, qty, 0.0, 0.0, TimeInForce.Day, 0, "");
			order.strategyId = (int)this.id;
			order.text = text;
			this.framework.OrderManager.Register(order);
			return order;
		}
		public Order SellOrder(Instrument instrument, double qty, string text = "")
		{
			Order order = new Order(this.GetExecutionProvider(instrument), this.portfolio, instrument, OrderType.Market, OrderSide.Sell, qty, 0.0, 0.0, TimeInForce.Day, 0, "");
			order.strategyId = (int)this.id;
			order.text = text;
			this.framework.OrderManager.Register(order);
			return order;
		}
		public Order BuyLimitOrder(Instrument instrument, double qty, double price, string text = "")
		{
			Order order = new Order(this.GetExecutionProvider(instrument), this.portfolio, instrument, OrderType.Limit, OrderSide.Buy, qty, 0.0, 0.0, TimeInForce.Day, 0, "");
			order.strategyId = (int)this.id;
			order.text = text;
			order.price = price;
			this.framework.OrderManager.Register(order);
			return order;
		}
		public Order SellLimitOrder(Instrument instrument, double qty, double price, string text = "")
		{
			Order order = new Order(this.GetExecutionProvider(instrument), this.portfolio, instrument, OrderType.Limit, OrderSide.Sell, qty, 0.0, 0.0, TimeInForce.Day, 0, "");
			order.strategyId = (int)this.id;
			order.text = text;
			order.price = price;
			this.framework.OrderManager.Register(order);
			return order;
		}
		public Order BuyStopOrder(Instrument instrument, double qty, double stopPx, string text = "")
		{
			Order order = new Order(this.GetExecutionProvider(instrument), this.portfolio, instrument, OrderType.Stop, OrderSide.Buy, qty, 0.0, 0.0, TimeInForce.Day, 0, "");
			order.strategyId = (int)this.id;
			order.text = text;
			order.stopPx = stopPx;
			this.framework.OrderManager.Register(order);
			return order;
		}
		public Order SellStopOrder(Instrument instrument, double qty, double stopPx, string text = "")
		{
			Order order = new Order(this.GetExecutionProvider(instrument), this.portfolio, instrument, OrderType.Stop, OrderSide.Sell, qty, 0.0, 0.0, TimeInForce.Day, 0, "");
			order.strategyId = (int)this.id;
			order.text = text;
			order.stopPx = stopPx;
			this.framework.OrderManager.Register(order);
			return order;
		}
		public Order Order(Instrument instrument, OrderType type, OrderSide side, double qty, double stopPx, double price, string text = "")
		{
			Order order = new Order(this.GetExecutionProvider(instrument), this.portfolio, instrument, type, side, qty, 0.0, 0.0, TimeInForce.Day, 0, "");
			order.strategyId = (int)this.id;
			order.text = text;
			order.stopPx = stopPx;
			order.price = price;
			this.framework.OrderManager.Register(order);
			return order;
		}
		public Order BuyStopLimitOrder(Instrument instrument, double qty, double stopPx, double price, string text = "")
		{
			Order order = new Order(this.GetExecutionProvider(instrument), this.portfolio, instrument, OrderType.StopLimit, OrderSide.Buy, qty, 0.0, 0.0, TimeInForce.Day, 0, "");
			order.strategyId = (int)this.id;
			order.text = text;
			order.stopPx = stopPx;
			order.price = price;
			this.framework.OrderManager.Register(order);
			return order;
		}
		public Order SellStopLimitOrder(Instrument instrument, double qty, double stopPx, double price, string text = "")
		{
			Order order = new Order(this.GetExecutionProvider(instrument), this.portfolio, instrument, OrderType.StopLimit, OrderSide.Sell, qty, 0.0, 0.0, TimeInForce.Day, 0, "");
			order.strategyId = (int)this.id;
			order.text = text;
			order.stopPx = stopPx;
			order.price = price;
			this.framework.OrderManager.Register(order);
			return order;
		}
		public void AddStop(Stop stop)
		{
			this.stops.Add(stop);
			if (this.stopsByInstrument[stop.instrument.Id] == null)
			{
				this.stopsByInstrument[stop.instrument.Id] = new List<Stop>();
			}
			this.stopsByInstrument[stop.instrument.Id].Add(stop);
		}
		public virtual double Objective()
		{
			return this.portfolio.Value;
		}
		public string GetStatusAsString()
		{
			switch (this.status)
			{
			case StrategyStatus.Running:
				return "Running";
			case StrategyStatus.Stopped:
				return "Stopped";
			default:
				return "Undefined";
			}
		}
		public string GetModeAsString()
		{
			switch (this.Mode)
			{
			case StrategyMode.Backtest:
				return "Backtest";
			case StrategyMode.Paper:
				return "Paper";
			case StrategyMode.Live:
				return "Live";
			default:
				return "Undefined";
			}
		}
		public void AddStrategy(Strategy strategy)
		{
			this.AddStrategy(strategy, true);
		}
		public void AddStrategy(Strategy strategy, bool callOnStrategyStart)
		{
			strategy.id = this.framework.strategyManager.GetNextId();
			strategy.parent = this;
			this.strategies.Add(strategy);
			strategy.status = this.status;
			if (this.status == StrategyStatus.Running)
			{
				this.RegisterStrategy(strategy, strategy.instruments, (int)strategy.id);
				if (callOnStrategyStart)
				{
					strategy.OnStrategyStart_();
				}
			}
		}
		internal void RegisterStrategy(Strategy strategy, InstrumentList instruments, int orderRouteId)
		{
			strategy.portfolio.Parent = this.portfolio;
			foreach (Instrument current in instruments)
			{
				LinkedList<Strategy> linkedList;
				if (this.strategiesByInstrument[current.Id] == null)
				{
					linkedList = new LinkedList<Strategy>();
					this.strategiesByInstrument[current.Id] = linkedList;
				}
				else
				{
					linkedList = this.strategiesByInstrument[current.Id];
				}
				linkedList.Add(strategy);
				IdArray<int> idArray;
				int num;
				(idArray = this.instrumentCountTable)[num = current.id] = idArray[num] + 1;
			}
			Dictionary<IDataProvider, InstrumentList> dictionary = new Dictionary<IDataProvider, InstrumentList>();
			foreach (Instrument current2 in instruments)
			{
				InstrumentList instrumentList = null;
				IDataProvider dataProvider = this.GetDataProvider(strategy, current2);
				IExecutionProvider executionProvider = strategy.GetExecutionProvider(current2);
				if (dataProvider.Status == ProviderStatus.Disconnected)
				{
					dataProvider.Connect();
				}
				if (executionProvider.Status == ProviderStatus.Disconnected)
				{
					executionProvider.Connect();
				}
				if (!dictionary.TryGetValue(dataProvider, out instrumentList))
				{
					instrumentList = new InstrumentList();
					dictionary[dataProvider] = instrumentList;
				}
				instrumentList.Add(current2);
			}
			foreach (KeyValuePair<IDataProvider, InstrumentList> current3 in dictionary)
			{
				this.framework.strategyManager.RegisterMarketDataRequest(current3.Key, current3.Value);
			}
			this.strategyByOrderId[orderRouteId] = strategy;
			if (this.parent != null)
			{
				this.parent.RegisterStrategy(this, instruments, orderRouteId);
			}
		}
		internal void UnregisterStrategy(Strategy strategy, InstrumentList instruments, int orderRouteId)
		{
			strategy.portfolio.Parent = this.portfolio;
			foreach (Instrument current in instruments)
			{
				LinkedList<Strategy> linkedList = this.strategiesByInstrument[current.Id];
				if (linkedList != null)
				{
					linkedList.Remove(strategy);
				}
				linkedList.Add(strategy);
				IdArray<int> idArray;
				int num;
				(idArray = this.instrumentCountTable)[num = current.id] = idArray[num] - 1;
				if (this.instrumentCountTable[current.id] == 0)
				{
					this.Instruments.Remove(current);
				}
			}
			Dictionary<IDataProvider, InstrumentList> dictionary = new Dictionary<IDataProvider, InstrumentList>();
			foreach (Instrument current2 in instruments)
			{
				InstrumentList instrumentList = null;
				IDataProvider key = this.GetDataProvider(strategy, current2);
				if (!dictionary.TryGetValue(key, out instrumentList))
				{
					instrumentList = new InstrumentList();
					dictionary[key] = instrumentList;
				}
				instrumentList.Add(current2);
			}
			foreach (KeyValuePair<IDataProvider, InstrumentList> current3 in dictionary)
			{
				this.framework.strategyManager.UnregisterMarketDataRequest(current3.Key, current3.Value);
			}
			this.strategyByOrderId[orderRouteId] = null;
			if (this.parent != null)
			{
				this.parent.UnregisterStrategy(this, instruments, orderRouteId);
			}
		}
		public void RemoveStrategy(Strategy strategy)
		{
		}
		public void Log(DataObject data, Group group)
		{
			this.framework.eventServer.OnLog(new GroupEvent(data, group));
		}
		public void Log(DataObject data, int groupId)
		{
			this.framework.eventServer.OnLog(new GroupEvent(data, this.framework.groupManager.Groups[groupId]));
		}
		public void Respond(DataObject data, int commandId)
		{
		}
		public void Respond(DataObject data)
		{
			this.Respond(data, -1);
		}
		internal IExecutionProvider GetExecutionProvider(Instrument instrument)
		{
			IExecutionProvider executionProvider = null;
			if (instrument.ExecutionProvider != null)
			{
				executionProvider = instrument.ExecutionProvider;
			}
			if (executionProvider == null && this.ExecutionProvider != null)
			{
				executionProvider = this.ExecutionProvider;
			}
			if (this.Mode != StrategyMode.Live)
			{
				if (executionProvider != null && executionProvider is SellSideStrategy)
				{
					return executionProvider;
				}
				return this.framework.providerManager.executionSimulator;
			}
			else
			{
				if (executionProvider != null)
				{
					return executionProvider;
				}
				return this.framework.ExecutionProvider;
			}
		}
		internal IDataProvider GetDataProvider(Strategy strategy, Instrument instrument)
		{
			IDataProvider dataProvider = null;
			if (instrument.DataProvider != null)
			{
				dataProvider = instrument.dataProvider;
			}
			if (dataProvider == null && strategy.DataProvider != null)
			{
				dataProvider = strategy.DataProvider;
			}
			if (this.framework.Mode == FrameworkMode.Simulation)
			{
				if (dataProvider != null && dataProvider is SellSideStrategy)
				{
					return dataProvider;
				}
				return this.framework.providerManager.dataSimulator;
			}
			else
			{
				if (dataProvider != null)
				{
					return dataProvider;
				}
				return this.framework.DataProvider;
			}
		}
	}
}
