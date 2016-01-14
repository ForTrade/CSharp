using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class EMA : Indicator
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
		public EMA(ISeries input, int length, BarData barData = BarData.Close) : base(input)
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
					"EMA (",
					this.length,
					", ",
					this.barData,
					")"
				});
			}
			else
			{
				this.name = "EMA (" + this.length + ")";
			}
			this.description = "Exponential Moving Average";
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
			double num = double.NaN;
			if (index >= 1)
			{
				double num2 = 2.0 / (double)(this.length + 1);
				double last = this.Last;
				num = last + num2 * (this.input[index, this.barData] - last);
			}
			else
			{
				if (index == 0)
				{
					num = this.input[0, this.barData];
				}
			}
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int length, BarData barData = BarData.Close)
		{
			if (index >= 1)
			{
				double num = 2.0 / (double)(length + 1);
				double num2 = EMA.Value(input, index - 1, length, barData);
				return num2 + num * (input[index, barData] - num2);
			}
			if (index == 0)
			{
				return input[0, barData];
			}
			return double.NaN;
		}
	}
}
