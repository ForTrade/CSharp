using System;
namespace SmartQuant
{
	public class TradeInfo
	{
		public DateTime EntryDate
		{
			get;
			set;
		}
		public DateTime ExitDate
		{
			get;
			set;
		}
		public double EntryPrice
		{
			get;
			set;
		}
		public double ExitPrice
		{
			get;
			set;
		}
		public double Qty
		{
			get;
			set;
		}
		public double EntryCost
		{
			get;
			set;
		}
		public double ExitCost
		{
			get;
			set;
		}
		public bool IsLong
		{
			get;
			set;
		}
		public bool IsWinning
		{
			get
			{
				return this.PnL > 0.0;
			}
		}
		public double PnL
		{
			get
			{
				double num = this.Qty * (this.ExitPrice - this.EntryPrice) * (double)(this.IsLong ? 1 : -1);
				return num - (this.EntryCost + this.ExitCost);
			}
		}
		public double NetPnL
		{
			get
			{
				return this.Qty * (this.ExitPrice - this.EntryPrice) * (double)(this.IsLong ? 1 : -1);
			}
		}
		public long Length
		{
			get
			{
				return (this.ExitDate - this.EntryDate).Ticks;
			}
		}
	}
}
