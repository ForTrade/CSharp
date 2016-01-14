using System;
namespace SmartQuant
{
	public class Fill : DataObject
	{
		internal Order order;
		internal Instrument instrument;
		internal int instrumentId;
		internal byte currencyId;
		internal OrderSide side;
		internal double qty;
		internal double price;
		internal string text;
		internal double commission;
		public override byte TypeId
		{
			get
			{
				return 10;
			}
		}
		public Order Order
		{
			get
			{
				return this.order;
			}
		}
		public Instrument Instrument
		{
			get
			{
				return this.instrument;
			}
		}
		public byte CurrencyId
		{
			get
			{
				return this.currencyId;
			}
		}
		public OrderSide Side
		{
			get
			{
				return this.side;
			}
		}
		public double Qty
		{
			get
			{
				return this.qty;
			}
		}
		public double Price
		{
			get
			{
				return this.price;
			}
		}
		public double Value
		{
			get
			{
				if (this.instrument.factor != 0.0)
				{
					return this.price * this.qty * this.instrument.factor;
				}
				return this.price * this.qty;
			}
		}
		public double NetCashFlow
		{
			get
			{
				if (this.side == OrderSide.Buy)
				{
					return -this.Value;
				}
				return this.Value;
			}
		}
		public double CashFlow
		{
			get
			{
				return this.NetCashFlow - this.commission;
			}
		}
		public string Text
		{
			get
			{
				return this.text;
			}
		}
		public double Commission
		{
			get
			{
				return this.commission;
			}
		}
		public Fill()
		{
		}
		public Fill(DateTime dateTime, Order order, Instrument instrument, byte currencyId, OrderSide side, double qty, double price, string text = "")
		{
			this.dateTime = dateTime;
			this.order = order;
			this.instrument = instrument;
			this.currencyId = currencyId;
			this.side = side;
			this.qty = qty;
			this.price = price;
			this.text = text;
		}
		public Fill(ExecutionReport report)
		{
			this.dateTime = report.dateTime;
			this.order = report.order;
			this.instrument = report.instrument;
			this.currencyId = report.currencyId;
			this.side = report.side;
			this.qty = report.lastQty;
			this.price = report.avgPx;
			this.commission = report.commission;
			this.text = report.text;
		}
		public Fill(Fill fill)
		{
			this.dateTime = fill.dateTime;
			this.order = fill.order;
			this.instrument = fill.instrument;
			this.currencyId = fill.currencyId;
			this.side = fill.side;
			this.qty = fill.qty;
			this.price = fill.price;
			this.commission = fill.commission;
			this.text = fill.text;
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
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this.dateTime,
				" ",
				this.GetSideAsString(),
				" ",
				this.instrument.symbol,
				" ",
				this.qty,
				" ",
				this.price,
				" ",
				this.text
			});
		}
	}
}
