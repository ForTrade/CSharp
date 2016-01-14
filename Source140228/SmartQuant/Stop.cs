using System;
namespace SmartQuant
{
	public class Stop
	{
		private Strategy strategy;
		private bool connected;
		protected internal StopType type = StopType.Trailing;
		protected internal StopMode mode = StopMode.Percent;
		protected internal StopStatus status;
		protected internal Position position;
		protected internal Instrument instrument;
		protected internal double level;
		protected internal double initPrice;
		protected internal double currPrice;
		protected internal double stopPrice;
		protected internal double fillPrice;
		protected internal double trailPrice;
		protected internal double qty;
		protected internal PositionSide side;
		protected internal DateTime creationTime;
		protected internal DateTime completionTime;
		protected internal bool traceOnQuote = true;
		protected internal bool traceOnTrade = true;
		protected internal bool traceOnBar = true;
		protected internal bool traceOnBarOpen = true;
		protected internal bool trailOnOpen = true;
		protected internal bool trailOnHighLow;
		protected internal long filterBarSize = -1L;
		protected internal BarType filterBarType = BarType.Time;
		protected internal StopFillMode fillMode = StopFillMode.Stop;
		public StopFillMode FillMode
		{
			get
			{
				return this.fillMode;
			}
			set
			{
				this.fillMode = value;
			}
		}
		public Instrument Instrument
		{
			get
			{
				return this.instrument;
			}
		}
		public bool Connected
		{
			get
			{
				return this.connected;
			}
		}
		public Stop(Strategy strategy, Position position, double level, StopType type, StopMode mode)
		{
			this.strategy = strategy;
			this.position = position;
			this.instrument = position.instrument;
			this.qty = position.qty;
			this.side = position.Side;
			this.level = level;
			this.type = type;
			this.mode = mode;
			this.currPrice = this.GetInstrumentPrice();
			this.trailPrice = this.currPrice;
			this.stopPrice = this.GetStopPrice();
			this.creationTime = strategy.framework.Clock.DateTime;
			this.completionTime = DateTime.MinValue;
			this.Connect();
		}
		public Stop(Strategy strategy, Position position, DateTime time)
		{
			this.strategy = strategy;
			this.position = position;
			this.instrument = position.instrument;
			this.qty = position.qty;
			this.side = position.Side;
			this.type = StopType.Time;
			this.creationTime = strategy.framework.Clock.DateTime;
			this.completionTime = time;
			this.stopPrice = this.GetInstrumentPrice();
			if (this.completionTime > this.creationTime)
			{
				strategy.framework.Clock.AddReminder(new Reminder(new ReminderCallback(this.OnClock), this.completionTime, null));
			}
		}
		private double GetInstrumentPrice()
		{
			if (this.position.Side == PositionSide.Long)
			{
				Bid bid = this.strategy.framework.dataManager.GetBid(this.instrument);
				if (bid != null)
				{
					return bid.Price;
				}
			}
			if (this.position.Side == PositionSide.Short)
			{
				Ask ask = this.strategy.framework.dataManager.GetAsk(this.instrument);
				if (ask != null)
				{
					return ask.Price;
				}
			}
			Trade trade = this.strategy.framework.dataManager.GetTrade(this.instrument);
			if (trade != null)
			{
				return trade.Price;
			}
			Bar bar = this.strategy.framework.dataManager.GetBar(this.instrument);
			if (bar != null)
			{
				return bar.Close;
			}
			return 0.0;
		}
		private double GetStopPrice()
		{
			this.initPrice = this.trailPrice;
			switch (this.mode)
			{
			case StopMode.Absolute:
				switch (this.side)
				{
				case PositionSide.Long:
					return this.trailPrice - Math.Abs(this.level);
				case PositionSide.Short:
					return this.trailPrice + Math.Abs(this.level);
				default:
					throw new ArgumentException("Unknown position side : " + this.position.Side);
				}
				break;
			case StopMode.Percent:
				switch (this.position.Side)
				{
				case PositionSide.Long:
					return this.trailPrice - Math.Abs(this.trailPrice * this.level);
				case PositionSide.Short:
					return this.trailPrice + Math.Abs(this.trailPrice * this.level);
				default:
					throw new ArgumentException("Unknown position side : " + this.position.Side);
				}
				break;
			default:
				throw new ArgumentException("Unknown stop mode : " + this.mode);
			}
		}
		public void Cancel()
		{
			if (this.status != StopStatus.Active)
			{
				return;
			}
			this.Disconnect();
			this.Complete(StopStatus.Canceled);
		}
		private void Connect()
		{
			this.connected = true;
		}
		public void Disconnect()
		{
			this.connected = false;
		}
		private void CheckStop()
		{
			if (this.currPrice == 0.0)
			{
				return;
			}
			switch (this.side)
			{
			case PositionSide.Long:
				if (this.currPrice <= this.stopPrice)
				{
					this.Disconnect();
					this.Complete(StopStatus.Executed);
					return;
				}
				if (this.type == StopType.Trailing && this.trailPrice > this.initPrice)
				{
					this.stopPrice = this.GetStopPrice();
					return;
				}
				break;
			case PositionSide.Short:
				if (this.currPrice >= this.stopPrice)
				{
					this.Disconnect();
					this.Complete(StopStatus.Executed);
					return;
				}
				if (this.type == StopType.Trailing && this.trailPrice < this.initPrice)
				{
					this.stopPrice = this.GetStopPrice();
				}
				break;
			default:
				return;
			}
		}
		internal void OnPositionClosed(Position position)
		{
			if (this.position == position)
			{
				this.Disconnect();
				this.Complete(StopStatus.Canceled);
			}
		}
		internal void OnBar(Bar bar)
		{
			if (this.traceOnBar && (this.filterBarSize < 0L || (this.filterBarSize == bar.Size && this.filterBarType == BarType.Time)))
			{
				this.trailPrice = bar.Close;
				switch (this.side)
				{
				case PositionSide.Long:
					this.currPrice = bar.Low;
					this.fillPrice = bar.Low;
					if (this.trailOnHighLow)
					{
						this.trailPrice = bar.High;
					}
					break;
				case PositionSide.Short:
					this.currPrice = bar.High;
					this.fillPrice = bar.High;
					if (this.trailOnHighLow)
					{
						this.trailPrice = bar.Low;
					}
					break;
				}
				switch (this.fillMode)
				{
				case StopFillMode.Close:
					this.fillPrice = bar.Close;
					break;
				case StopFillMode.Stop:
					this.fillPrice = this.stopPrice;
					break;
				}
				this.CheckStop();
			}
		}
		internal void OnBarOpen(Bar bar)
		{
			if (this.traceOnBar && this.traceOnBarOpen && (this.filterBarSize < 0L || (this.filterBarSize == bar.Size && this.filterBarType == BarType.Time)))
			{
				this.currPrice = bar.Open;
				this.fillPrice = bar.Open;
				if (this.trailOnOpen)
				{
					this.trailPrice = bar.Open;
				}
				this.CheckStop();
			}
		}
		internal void OnTrade(Trade trade)
		{
			if (this.traceOnTrade)
			{
				this.currPrice = trade.Price;
				this.fillPrice = trade.Price;
				this.trailPrice = trade.Price;
				this.CheckStop();
			}
		}
		internal void OnBid(Bid bid)
		{
			if (this.traceOnQuote && this.side == PositionSide.Long)
			{
				this.currPrice = bid.price;
				this.fillPrice = bid.price;
				this.trailPrice = bid.price;
				this.CheckStop();
			}
		}
		internal void OnAsk(Ask ask)
		{
			if (this.traceOnQuote && this.side == PositionSide.Short)
			{
				this.currPrice = ask.price;
				this.fillPrice = ask.price;
				this.trailPrice = ask.price;
				this.CheckStop();
			}
		}
		private void Complete(StopStatus status)
		{
			this.status = status;
			this.completionTime = this.strategy.framework.Clock.DateTime;
			this.strategy.OnStopStatusChanged_(this);
		}
		private void OnClock(DateTime dateTime, object data)
		{
			this.stopPrice = this.GetInstrumentPrice();
			this.Complete(StopStatus.Executed);
		}
	}
}
