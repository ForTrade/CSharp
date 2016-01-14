using System;
using System.Collections.Generic;
namespace SmartQuant
{
	public class Position
	{
		internal Portfolio portfolio;
		internal Instrument instrument;
		internal double amount;
		internal double qty;
		internal double qtyBought;
		internal double qtySold;
		internal List<Fill> fills = new List<Fill>();
		internal Fill entry;
		public Portfolio Portfolio
		{
			get
			{
				return this.portfolio;
			}
		}
		public Instrument Instrument
		{
			get
			{
				return this.instrument;
			}
		}
		public List<Fill> Fills
		{
			get
			{
				return this.fills;
			}
		}
		public double Amount
		{
			get
			{
				return this.amount;
			}
		}
		public double Qty
		{
			get
			{
				return this.qty;
			}
		}
		public double QtyBought
		{
			get
			{
				return this.qtyBought;
			}
		}
		public double QtySold
		{
			get
			{
				return this.qtySold;
			}
		}
		public PositionSide Side
		{
			get
			{
				if (this.amount < 0.0)
				{
					return PositionSide.Short;
				}
				return PositionSide.Long;
			}
		}
		public double Price
		{
			get
			{
				return this.portfolio.Pricer.GetPrice(this);
			}
		}
		public double Value
		{
			get
			{
				if (this.instrument.factor != 0.0)
				{
					return this.Price * this.amount * this.instrument.factor;
				}
				return this.Price * this.amount;
			}
		}
		public double EntryPrice
		{
			get
			{
				return this.entry.price;
			}
		}
		public DateTime EntryDate
		{
			get
			{
				return this.entry.dateTime;
			}
		}
		public double EntryQty
		{
			get
			{
				return this.entry.qty;
			}
		}
		public Position(Portfolio portfolio, Instrument instrument)
		{
			this.portfolio = portfolio;
			this.instrument = instrument;
		}
		public void Add(Fill fill)
		{
			this.fills.Add(fill);
			if (this.qty == 0.0)
			{
				this.entry = fill;
			}
			if (fill.Side == OrderSide.Buy)
			{
				this.qtyBought += fill.qty;
			}
			else
			{
				this.qtySold += fill.qty;
			}
			this.amount = this.qtyBought - this.qtySold;
			if (this.amount > 0.0)
			{
				this.qty = this.amount;
				return;
			}
			this.qty = -this.amount;
		}
		public string GetSideAsString()
		{
			switch (this.Side)
			{
			case PositionSide.Long:
				return "Long";
			case PositionSide.Short:
				return "Short";
			default:
				return "Undefined";
			}
		}
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this.instrument,
				" ",
				this.Side,
				" ",
				this.qty
			});
		}
	}
}
