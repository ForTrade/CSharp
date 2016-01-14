using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class WMA : Indicator
	{
		protected int length;
		protected BarData barData;
		[Category("Parameters"), Description("Length of Weighted Moving Average")]
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
		[Category("Parameters"), Description("Which type of data to average")]
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
		public WMA(ISeries input, int length, BarData barData = BarData.Close) : base(input)
		{
			this.length = length;
			this.barData = barData;
			this.Init();
		}
		protected override void Init()
		{
			if (this.input is BarSeries)
			{
				this.name = string.Concat(new object[]
				{
					"WMA (",
					this.length,
					", ",
					this.barData,
					")"
				});
			}
			else
			{
				this.name = "WMA (" + this.length + ")";
			}
			this.description = "Weighted Moving Average";
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
			double num = WMA.Value(this.input, index, this.length, this.barData);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int length, BarData barData = BarData.Close)
		{
			if (index >= length - 1)
			{
				double num = 0.0;
				for (int num2 = 1; num2 != length + 1; num2++)
				{
					num += (double)num2;
				}
				double num3 = 0.0;
				int num4 = index;
				int num5 = length;
				while (index - (length - 1) <= num4)
				{
					num3 += input[num4, barData] * (double)num5;
					num4--;
					num5--;
				}
				return num3 / num;
			}
			return double.NaN;
		}
	}
}
