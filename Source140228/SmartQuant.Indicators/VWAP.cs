using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class VWAP : Indicator
	{
		protected int length;
		protected BarData barData;
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
		public VWAP(ISeries input, int length, BarData barData = BarData.Close) : base(input)
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
					"VWAP (",
					this.length,
					", ",
					this.barData,
					")"
				});
			}
			else
			{
				this.name = "VWAP (" + this.length + ")";
			}
			this.description = "Volume Weighted Average Price";
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
			double num = VWAP.Value(this.input, index, this.length, this.barData);
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
				double num2 = 0.0;
				for (int i = index; i >= index - length + 1; i--)
				{
					num += input[i, barData] * input[i, BarData.Volume];
					num2 += input[i, BarData.Volume];
				}
				return num / num2;
			}
			return double.NaN;
		}
	}
}
