using System;
using System.Collections.Generic;
using System.ComponentModel;
namespace SmartQuant
{
	public class Order : DataObject
	{
		internal int id;
		internal short providerId;
		internal short portfolioId;
		public int strategyId;
		internal DateTime transactTime;
		internal Instrument instrument;
		internal IExecutionProvider provider;
		internal Portfolio portfolio;
		internal OrderStatus status;
		internal OrderSide side;
		internal OrderType type;
		internal TimeInForce timeInForce;
		internal DateTime expireTime;
		internal byte route;
		internal double price;
		internal double stopPx;
		internal double avgPx;
		internal double qty;
		internal double cumQty;
		internal double leavesQty;
		internal string oCA;
		internal string text;
		internal double queueSize;
		internal bool isOutPrice;
		internal bool isQueueCalculated;
		private List<ExecutionCommand> commands;
		private List<ExecutionReport> reports;
		private List<ExecutionMessage> messages;
		public override byte TypeId
		{
			get
			{
				return 12;
			}
		}
		[Category("Message"), Description("Reports")]
		public List<ExecutionReport> Reports
		{
			get
			{
				return this.reports;
			}
			set
			{
				this.reports = value;
			}
		}
		[Category("Message"), Description("Commands")]
		public List<ExecutionCommand> Commands
		{
			get
			{
				return this.commands;
			}
			set
			{
				this.commands = value;
			}
		}
		[Category("Message"), Description("Messages")]
		public List<ExecutionMessage> Messages
		{
			get
			{
				return this.messages;
			}
			set
			{
				this.messages = value;
			}
		}
		public int Id
		{
			get
			{
				return this.id;
			}
		}
		[Browsable(false)]
		public Portfolio Portfolio
		{
			get
			{
				return this.portfolio;
			}
		}
		[Browsable(false)]
		public IExecutionProvider Provider
		{
			get
			{
				return this.provider;
			}
			set
			{
				this.provider = value;
			}
		}
		public string OCA
		{
			get
			{
				return this.oCA;
			}
			set
			{
				this.oCA = value;
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
		[Browsable(false)]
		public bool IsNotSent
		{
			get
			{
				return this.status == OrderStatus.NotSent;
			}
		}
		[Browsable(false)]
		public bool IsPendingNew
		{
			get
			{
				return this.status == OrderStatus.PendingNew;
			}
		}
		[Browsable(false)]
		public bool IsNew
		{
			get
			{
				return this.status == OrderStatus.New;
			}
		}
		[Browsable(false)]
		public bool IsRejected
		{
			get
			{
				return this.status == OrderStatus.Rejected;
			}
		}
		[Browsable(false)]
		public bool IsPartiallyFilled
		{
			get
			{
				return this.status == OrderStatus.PartiallyFilled;
			}
		}
		[Browsable(false)]
		public bool IsFilled
		{
			get
			{
				return this.status == OrderStatus.Filled;
			}
		}
		[Browsable(false)]
		public bool IsPendingCancel
		{
			get
			{
				return this.status == OrderStatus.PendingCancel;
			}
		}
		[Browsable(false)]
		public bool IsCancelled
		{
			get
			{
				return this.status == OrderStatus.Cancelled;
			}
		}
		[Browsable(false)]
		public bool IsPendingReplace
		{
			get
			{
				return this.status == OrderStatus.PendingReplace;
			}
		}
		[Browsable(false)]
		public bool IsReplaced
		{
			get
			{
				return this.status == OrderStatus.Replaced;
			}
		}
		[Browsable(false)]
		public bool IsDone
		{
			get
			{
				return this.status == OrderStatus.Filled || this.status == OrderStatus.Cancelled || this.status == OrderStatus.Rejected;
			}
		}
		[Browsable(false)]
		public bool IsOCA
		{
			get
			{
				return !string.IsNullOrEmpty(this.oCA);
			}
		}
		public OrderStatus Status
		{
			get
			{
				return this.status;
			}
		}
		public Instrument Instrument
		{
			get
			{
				return this.instrument;
			}
		}
		public double StopPx
		{
			get
			{
				return this.stopPx;
			}
		}
		public double Price
		{
			get
			{
				return this.price;
			}
		}
		public OrderSide Side
		{
			get
			{
				return this.side;
			}
		}
		public OrderType Type
		{
			get
			{
				return this.type;
			}
		}
		public double Qty
		{
			get
			{
				return this.qty;
			}
		}
		public double CumQty
		{
			get
			{
				return this.cumQty;
			}
		}
		public double LeavesQty
		{
			get
			{
				return this.leavesQty;
			}
		}
		public double AvgPx
		{
			get
			{
				return this.avgPx;
			}
		}
		public TimeInForce TimeInForce
		{
			get
			{
				return this.timeInForce;
			}
		}
		public DateTime ExpireTime
		{
			get
			{
				return this.expireTime;
			}
		}
		public byte Route
		{
			get
			{
				return this.route;
			}
		}
		public DateTime TransactTime
		{
			get
			{
				return this.transactTime;
			}
		}
		public Order(IExecutionProvider provider, Instrument instrument, OrderType type, OrderSide side, double qty, double price = 0.0, double stopPx = 0.0, TimeInForce timeInForce = TimeInForce.Day, string text = "") : this()
		{
			this.provider = provider;
			this.instrument = instrument;
			this.type = type;
			this.side = side;
			this.qty = qty;
			this.price = price;
			this.stopPx = stopPx;
			this.timeInForce = timeInForce;
			this.text = text;
			this.portfolio = null;
		}
		public Order(IExecutionProvider provider, Portfolio portfolio, Instrument instrument, OrderType type, OrderSide side, double qty, double price = 0.0, double stopPx = 0.0, TimeInForce timeInForce = TimeInForce.Day, byte route = 0, string text = "") : this()
		{
			this.provider = provider;
			this.portfolio = portfolio;
			this.instrument = instrument;
			this.type = type;
			this.side = side;
			this.qty = qty;
			this.price = price;
			this.stopPx = stopPx;
			this.timeInForce = timeInForce;
			this.route = route;
			this.text = text;
		}
		public Order(Order order) :base()
		{
			this.id = -1;
			this.oCA = "";
			this.text = "";
			this.id = order.id;
			this.providerId = order.providerId;
			this.portfolioId = order.portfolioId;
			this.transactTime = order.transactTime;
			this.dateTime = order.dateTime;
			this.instrument = order.instrument;
			this.provider = order.provider;
			this.portfolio = order.portfolio;
			this.status = order.status;
			this.side = order.side;
			this.type = order.type;
			this.timeInForce = order.timeInForce;
			this.price = order.price;
			this.stopPx = order.stopPx;
			this.avgPx = order.avgPx;
			this.qty = order.qty;
			this.cumQty = order.cumQty;
			this.text = order.text;
			this.commands = order.commands;
			this.reports = order.reports;
			this.messages = order.messages;
		}
		public Order() :base()
		{
			this.id = -1;
			this.oCA = "";
			this.text = "";
			this.commands = new List<ExecutionCommand>();
			this.reports = new List<ExecutionReport>();
			this.messages = new List<ExecutionMessage>();
		}
		public void OnExecutionCommand(ExecutionCommand command)
		{
			this.commands.Add(command);
			this.messages.Add(command);
		}
		public void OnExecutionReport(ExecutionReport report)
		{
			this.status = report.ordStatus;
			this.cumQty = report.cumQty;
			this.leavesQty = report.leavesQty;
			this.avgPx = report.avgPx;
			if (report.execType == ExecType.ExecReplace)
			{
				this.type = report.ordType;
				this.price = report.price;
				this.stopPx = report.stopPx;
				this.qty = report.ordQty;
			}
			this.reports.Add(report);
			this.messages.Add(report);
		}
		public string GetSideAsString()
		{
			switch (this.side)
			{
			case OrderSide.Buy:
				return "Buy";
			case OrderSide.Sell:
				return "Sell";
			default:
				return "Undefined";
			}
		}
		public string GetTypeAsString()
		{
			switch (this.type)
			{
			case OrderType.Market:
				return "Market";
			case OrderType.Stop:
				return "Stop";
			case OrderType.Limit:
				return "Limit";
			case OrderType.StopLimit:
				return "StopLimit";
			default:
				return "Undefined";
			}
		}
		public string GetStatusAsString()
		{
			switch (this.status)
			{
			case OrderStatus.NotSent:
				return "NotSent";
			case OrderStatus.PendingNew:
				return "PendingNew";
			case OrderStatus.New:
				return "New";
			case OrderStatus.Rejected:
				return "Rejected";
			case OrderStatus.PartiallyFilled:
				return "PartiallyFilled";
			case OrderStatus.Filled:
				return "Filled";
			case OrderStatus.PendingCancel:
				return "PendingCancel";
			case OrderStatus.Cancelled:
				return "Cancelled";
			case OrderStatus.PendingReplace:
				return "PendingReplace";
			case OrderStatus.Replaced:
				return "Replaced";
			}
			return "Undefined";
		}
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.dateTime.ToString(),
				" ",
				this.GetTypeAsString(),
				" ",
				this.GetSideAsString()
			});
		}
	}
}
