using System;
namespace SmartQuant
{
	public class ExecutionCommand : ExecutionMessage
	{
		internal Order order;
		internal ExecutionCommandType type;
		internal int id;
		internal short providerId;
		internal short portfolioId;
		internal DateTime transactTime;
		internal Instrument instrument;
		internal int instrumentId;
		internal IExecutionProvider provider;
		internal Portfolio portfolio;
		internal OrderSide side;
		internal OrderType orderType;
		internal TimeInForce timeInForce;
		internal double price;
		internal double stopPx;
		internal double qty;
		internal string oCA = "";
		internal string text = "";
		public override byte TypeId
		{
			get
			{
				return 14;
			}
		}
		public int Id
		{
			get
			{
				return this.id;
			}
		}
		public Portfolio Portfolio
		{
			get
			{
				return this.portfolio;
			}
		}
		public IExecutionProvider Provider
		{
			get
			{
				return this.provider;
			}
		}
		public string OCA
		{
			get
			{
				return this.oCA;
			}
		}
		public string Text
		{
			get
			{
				return this.text;
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
		public OrderType OrdType
		{
			get
			{
				return this.orderType;
			}
		}
		public double Qty
		{
			get
			{
				return this.qty;
			}
		}
		public DateTime TransactTime
		{
			get
			{
				return this.transactTime;
			}
		}
		public Order Order
		{
			get
			{
				return this.order;
			}
		}
		public ExecutionCommandType Type
		{
			get
			{
				return this.type;
			}
		}
		public ExecutionCommand()
		{
		}
		public ExecutionCommand(ExecutionCommandType type, Order order)
		{
			this.type = type;
			this.order = order;
			this.id = order.id;
		}
		public ExecutionCommand(ExecutionCommand command)
		{
			this.id = command.id;
			this.providerId = command.providerId;
			this.portfolioId = command.portfolioId;
			this.transactTime = command.transactTime;
			this.dateTime = command.dateTime;
			this.instrument = command.instrument;
			this.provider = command.provider;
			this.portfolio = command.portfolio;
			this.side = command.side;
			this.orderType = command.OrdType;
			this.timeInForce = command.timeInForce;
			this.price = command.price;
			this.stopPx = command.stopPx;
			this.qty = command.qty;
			this.oCA = command.oCA;
			this.text = command.text;
		}
	}
}
