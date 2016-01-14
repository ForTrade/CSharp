using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class SMA : Indicator
	{
		protected int length;
		protected BarData barData;
		[Category("Parameters"), Description("")]
		public BarData BarData
		{
			get
			{
				return this.barData;
			}
			set
			{
				this.barData = value;
				this.Init();
			}
		}
		[Category("Parameters"), Description("")]
		public int Length
		{
			get
			{
				return this.length;
			}
			set
			{
				this.length = value;
				this.Init();
			}
		}
		public SMA(ISeries input, int length, BarData barData = BarData.Close) : base(input)
		{
			this.length = length;
			this.barData = barData;
			this.Init();
		}
		protected override void Init()
		{
			this.name = "SMA (" + this.length + ")";
			this.description = "Simple Moving Average";
			base.Clear();
			this.calculate = true;
		}
		protected internal override void Calculate(int index)
		{
			if (this.calculate)
			{
				this.Calculate();
				return;
			}
			if (index >= this.length - 1)
			{
				double num = 0.0;
				if (index == this.length - 1)
				{
					for (int i = index; i >= index - this.length + 1; i--)
					{
						num += this.input[i, this.barData] / (double)this.length;
					}
				}
				else
				{
					num = base[this.input.GetDateTime(index - 1), SearchOption.Exact] + (this.input[index, this.barData] - this.input[index - this.length, this.barData]) / (double)this.length;
				}
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int length, BarData barData = BarData.Close)
		{
			if (index >= length - 1)
			{
				double num = 0.0;
				for (int i = index; i >= index - length + 1; i--)
				{
					num += input[i, barData];
				}
				return num / (double)length;
			}
			return double.NaN;
		}
	}
}
