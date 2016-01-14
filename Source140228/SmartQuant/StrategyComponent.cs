using System;
namespace SmartQuant
{
	public class StrategyComponent
	{
		protected internal Framework framework;
		protected internal ComponentStrategy strategy;
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
		public Instrument Instrument
		{
			get
			{
				return this.strategy.Instrument;
			}
		}
		public Position Position
		{
			get
			{
				return this.strategy.Position;
			}
		}
		public BarSeries Bars
		{
			get
			{
				return this.strategy.Bars;
			}
		}
		public TimeSeries Equity
		{
			get
			{
				return this.strategy.Equity;
			}
		}
		public Portfolio Portfolio
		{
			get
			{
				return this.strategy.Portfolio;
			}
		}
		public void Log(DataObject data, Group group)
		{
			this.strategy.Log(data, group);
		}
		public void Log(DataObject data, int groupId)
		{
			this.strategy.Log(data, groupId);
		}
		public bool HasPosition(PositionSide side, double qty)
		{
			return this.strategy.HasPosition(this.Instrument, side, qty);
		}
		public bool HasPosition()
		{
			return this.strategy.HasPosition(this.Instrument);
		}
		public bool HasLongPosition(double qty)
		{
			return this.strategy.HasLongPosition(this.Instrument, qty);
		}
		public bool HasLongPosition()
		{
			return this.strategy.HasLongPosition(this.Instrument);
		}
		public bool HasShortPosition(double qty)
		{
			return this.strategy.HasShortPosition(this.Instrument, qty);
		}
		public bool HasShortPosition()
		{
			return this.strategy.HasShortPosition(this.Instrument);
		}
		public virtual void OnStrategyStart()
		{
		}
		public virtual void OnReminder(DateTime dateTime, object data)
		{
		}
		public void Signal(double value)
		{
			Signal signal = new Signal(value);
			this.strategy.PositionComponent.OnSignal(signal);
		}
		public void Buy(double qty)
		{
			Order order = new Order(this.strategy.ExecutionProvider, this.strategy.Portfolio, this.strategy.Instrument, OrderType.Market, OrderSide.Buy, qty, 0.0, 0.0, TimeInForce.Day, 0, "");
			order.strategyId = (int)this.strategy.id;
			this.strategy.ExecutionComponent.OnOrder(order);
		}
		public void Buy(double qty, string text)
		{
			Order order = new Order(this.strategy.ExecutionProvider, this.strategy.Portfolio, this.strategy.Instrument, OrderType.Market, OrderSide.Buy, qty, 0.0, 0.0, TimeInForce.Day, 0, "");
			order.text = text;
			order.strategyId = (int)this.strategy.id;
			this.strategy.ExecutionComponent.OnOrder(order);
		}
		public void Sell(double qty)
		{
			Order order = new Order(this.strategy.ExecutionProvider, this.strategy.Portfolio, this.strategy.Instrument, OrderType.Market, OrderSide.Sell, qty, 0.0, 0.0, TimeInForce.Day, 0, "");
			order.strategyId = (int)this.strategy.id;
			this.strategy.ExecutionComponent.OnOrder(order);
		}
		public void Sell(double qty, string text)
		{
			Order order = new Order(this.strategy.ExecutionProvider, this.strategy.Portfolio, this.strategy.Instrument, OrderType.Market, OrderSide.Sell, qty, 0.0, 0.0, TimeInForce.Day, 0, "");
			order.text = text;
			order.strategyId = (int)this.strategy.id;
			this.strategy.ExecutionComponent.OnOrder(order);
		}
		public void BuyLimit(double qty, double price)
		{
			Order order = new Order(this.strategy.ExecutionProvider, this.strategy.Portfolio, this.strategy.Instrument, OrderType.Limit, OrderSide.Buy, qty, price, 0.0, TimeInForce.Day, 0, "");
			order.strategyId = (int)this.strategy.id;
			this.strategy.ExecutionComponent.OnOrder(order);
		}
		public void BuyLimit(double qty, double price, string text)
		{
			Order order = new Order(this.strategy.ExecutionProvider, this.strategy.Portfolio, this.strategy.Instrument, OrderType.Limit, OrderSide.Buy, qty, price, 0.0, TimeInForce.Day, 0, text);
			order.strategyId = (int)this.strategy.id;
			this.strategy.ExecutionComponent.OnOrder(order);
		}
		public void SellLimit(double qty, double price)
		{
			Order order = new Order(this.strategy.ExecutionProvider, this.strategy.Portfolio, this.strategy.Instrument, OrderType.Limit, OrderSide.Sell, qty, price, 0.0, TimeInForce.Day, 0, "");
			order.strategyId = (int)this.strategy.id;
			this.strategy.ExecutionComponent.OnOrder(order);
		}
		public void SellLimit(double qty, double price, string text)
		{
			Order order = new Order(this.strategy.ExecutionProvider, this.strategy.Portfolio, this.strategy.Instrument, OrderType.Limit, OrderSide.Sell, qty, price, 0.0, TimeInForce.Day, 0, text);
			order.strategyId = (int)this.strategy.id;
			this.strategy.ExecutionComponent.OnOrder(order);
		}
		public void BuyStop(double qty, double stopPx)
		{
			Order order = new Order(this.strategy.ExecutionProvider, this.strategy.Portfolio, this.strategy.Instrument, OrderType.Stop, OrderSide.Buy, qty, 0.0, stopPx, TimeInForce.Day, 0, "");
			order.strategyId = (int)this.strategy.id;
			this.strategy.ExecutionComponent.OnOrder(order);
		}
		public void BuyStop(double qty, double stopPx, string text)
		{
			Order order = new Order(this.strategy.ExecutionProvider, this.strategy.Portfolio, this.strategy.Instrument, OrderType.Stop, OrderSide.Buy, qty, 0.0, stopPx, TimeInForce.Day, 0, text);
			order.strategyId = (int)this.strategy.id;
			this.strategy.ExecutionComponent.OnOrder(order);
		}
		public void SellStop(double qty, double stopPx)
		{
			Order order = new Order(this.strategy.ExecutionProvider, this.strategy.Portfolio, this.strategy.Instrument, OrderType.Stop, OrderSide.Sell, qty, 0.0, stopPx, TimeInForce.Day, 0, "");
			order.strategyId = (int)this.strategy.id;
			this.strategy.ExecutionComponent.OnOrder(order);
		}
		public void SellStop(double qty, double stopPx, string text)
		{
			Order order = new Order(this.strategy.ExecutionProvider, this.strategy.Portfolio, this.strategy.Instrument, OrderType.Stop, OrderSide.Sell, qty, 0.0, stopPx, TimeInForce.Day, 0, text);
			order.strategyId = (int)this.strategy.id;
			this.strategy.ExecutionComponent.OnOrder(order);
		}
		public Stop SetStop(double level, StopType type = StopType.Fixed, StopMode mode = StopMode.Absolute)
		{
			Stop stop = new Stop(this.strategy, this.Position, level, type, mode);
			this.strategy.AddStop(stop);
			return stop;
		}
		public void AddReminder(DateTime dateTime, object data = null)
		{
			this.strategy.framework.Clock.AddReminder(new ReminderCallback(this.OnReminder), dateTime, data);
		}
	}
}
