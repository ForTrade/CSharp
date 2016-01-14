using System;
namespace SmartQuant
{
	public class ExecutionReport : ExecutionMessage
	{
		internal int commandID;
		internal Order order;
		internal Instrument instrument;
		internal int instrumentId;
		internal byte currencyId;
		internal ExecType execType;
		internal OrderType ordType;
		internal OrderSide side;
		internal TimeInForce timeInForce;
		internal OrderStatus ordStatus;
		internal double lastPx;
		internal double avgPx;
		internal double ordQty;
		internal double cumQty;
		internal double lastQty;
		internal double leavesQty;
		internal double price;
		internal double stopPx;
		internal double commission;
		internal string text;
		public override byte TypeId
		{
			get
			{
				return 13;
			}
		}
		public int CommandId
		{
			get
			{
				return this.commandID;
			}
			set
			{
				this.commandID = value;
			}
		}
		public Order Order
		{
			get
			{
				return this.order;
			}
			set
			{
				this.order = value;
			}
		}
		public Instrument Instrument
		{
			get
			{
				return this.instrument;
			}
			set
			{
				this.instrument = value;
			}
		}
		public byte CurrencyId
		{
			get
			{
				return this.currencyId;
			}
			set
			{
				this.currencyId = value;
			}
		}
		public ExecType ExecType
		{
			get
			{
				return this.execType;
			}
			set
			{
				this.execType = value;
			}
		}
		public OrderType OrdType
		{
			get
			{
				return this.ordType;
			}
			set
			{
				this.ordType = value;
			}
		}
		public OrderSide Side
		{
			get
			{
				return this.side;
			}
			set
			{
				this.side = value;
			}
		}
		public TimeInForce TimeInForce
		{
			get
			{
				return this.timeInForce;
			}
			set
			{
				this.timeInForce = value;
			}
		}
		public OrderStatus OrdStatus
		{
			get
			{
				return this.ordStatus;
			}
			set
			{
				this.ordStatus = value;
			}
		}
		public double LastPx
		{
			get
			{
				return this.lastPx;
			}
			set
			{
				this.lastPx = value;
			}
		}
		public double AvgPx
		{
			get
			{
				return this.avgPx;
			}
			set
			{
				this.avgPx = value;
			}
		}
		public double OrdQty
		{
			get
			{
				return this.ordQty;
			}
			set
			{
				this.ordQty = value;
			}
		}
		public double CumQty
		{
			get
			{
				return this.cumQty;
			}
			set
			{
				this.cumQty = value;
			}
		}
		public double LastQty
		{
			get
			{
				return this.lastQty;
			}
			set
			{
				this.lastQty = value;
			}
		}
		public double LeavesQty
		{
			get
			{
				return this.leavesQty;
			}
			set
			{
				this.leavesQty = value;
			}
		}
		public double Price
		{
			get
			{
				return this.price;
			}
			set
			{
				this.price = value;
			}
		}
		public double StopPx
		{
			get
			{
				return this.stopPx;
			}
			set
			{
				this.stopPx = value;
			}
		}
		public double Commission
		{
			get
			{
				return this.commission;
			}
			set
			{
				this.commission = value;
			}
		}
		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
			}
		}
		public ExecutionReport()
		{
		}
		public ExecutionReport(ExecutionReport report)
		{
			this.dateTime = report.dateTime;
			this.instrument = report.instrument;
			this.currencyId = report.currencyId;
			this.execType = report.execType;
			this.ordType = report.ordType;
			this.side = report.side;
			this.timeInForce = report.timeInForce;
			this.ordStatus = report.ordStatus;
			this.order = report.order;
			this.commandID = report.commandID;
			this.lastPx = report.lastPx;
			this.avgPx = report.avgPx;
			this.ordQty = report.ordQty;
			this.cumQty = report.cumQty;
			this.lastQty = report.lastQty;
			this.leavesQty = report.leavesQty;
			this.price = report.price;
			this.stopPx = report.stopPx;
			this.commission = report.commission;
		}
	}
}
