using System;
using System.Collections.Generic;
namespace SmartQuant
{
	public class TradeDetector
	{
		private IFillSet FillSet;
		private double leavesQty;
		private List<TradeInfo> trades;
		internal event TradeInfoEventHandler TradeDetected;
		public List<TradeInfo> Trades
		{
			get
			{
				return this.trades;
			}
		}
		public bool HasPosition
		{
			get
			{
				return this.FillSet.Peek() != null;
			}
		}
		public DateTime OpenDateTime
		{
			get
			{
				Fill fill = this.FillSet.Peek();
				if (fill == null)
				{
					return DateTime.MinValue;
				}
				return fill.DateTime;
			}
		}
		public TradeDetector(TradeDetectionType type)
		{
			if (type == TradeDetectionType.FIFO)
			{
				this.FillSet = new QueueFillSet();
			}
			else
			{
				this.FillSet = new StackFillSet();
			}
			this.trades = new List<TradeInfo>();
		}
		public void Add(Fill fill)
		{
			Fill fill2 = this.FillSet.Peek();
			if (fill2 == null || (this.IsLong(fill2) && this.IsLong(fill)) || (!this.IsLong(fill2) && !this.IsLong(fill)))
			{
				this.FillSet.Push(fill);
				this.leavesQty = fill.Qty;
				return;
			}
			double num = fill.Qty;
			while (num > 0.0 && (fill2 = this.FillSet.Peek()) != null)
			{
				if (this.leavesQty > num)
				{
					this.AddTrade(this.CreateTrade(fill2, fill, num));
					this.leavesQty -= Math.Round(num, 5);
					num = 0.0;
				}
				else
				{
					this.AddTrade(this.CreateTrade(fill2, fill, this.leavesQty));
					this.FillSet.Pop();
					num -= Math.Round(this.leavesQty, 5);
				}
			}
			if (num > 0.0)
			{
				this.leavesQty = num;
				this.FillSet.Push(fill);
			}
		}
		private void AddTrade(TradeInfo tradeInfo)
		{
			this.trades.Add(tradeInfo);
			if (this.TradeDetected != null)
			{
				this.TradeDetected(this, new TradeInfoEventArgs(tradeInfo));
			}
		}
		private TradeInfo CreateTrade(Fill oppFill, Fill Fill, double qty)
		{
			return new TradeInfo
			{
				EntryDate = oppFill.DateTime,
				EntryPrice = oppFill.Price,
				EntryCost = oppFill.Commission * qty / oppFill.Qty,
				ExitDate = Fill.DateTime,
				ExitPrice = Fill.Price,
				ExitCost = Fill.Commission * qty / Fill.Qty,
				Qty = qty,
				IsLong = this.IsLong(oppFill)
			};
		}
		private bool IsLong(Fill fill)
		{
			return fill.side == OrderSide.Buy;
		}
	}
}
