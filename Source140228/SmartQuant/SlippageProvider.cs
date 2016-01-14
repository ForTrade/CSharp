using System;
namespace SmartQuant
{
	public class SlippageProvider : ISlippageProvider
	{
		private double slippage;
		public double Slippage
		{
			get
			{
				return this.slippage;
			}
			set
			{
				this.slippage = value;
			}
		}
		public virtual double GetPrice(ExecutionReport report)
		{
			double num = report.AvgPx;
			switch (report.side)
			{
			case OrderSide.Buy:
				num += num * this.slippage;
				break;
			case OrderSide.Sell:
				num -= num * this.slippage;
				break;
			}
			return num;
		}
	}
}
