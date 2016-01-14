using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class DPO : Indicator
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
		public DPO(ISeries input, int length, BarData barData = BarData.Close) : base(input)
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
					"DPO (",
					this.length,
					", ",
					this.barData,
					")"
				});
			}
			else
			{
				this.name = "DPO (" + this.length + ")";
			}
			this.description = "Detrended Price Oscillator";
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
			double num = DPO.Value(this.input, index, this.length, this.barData);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int length, BarData barData)
		{
			if (index >= length / 2 + length - 1)
			{
				double num = input[index, barData];
				double num2 = 0.0;
				for (int i = index - length / 2; i > index - length - length / 2; i--)
				{
					num2 += input[i, barData];
				}
				num2 /= (double)length;
				return num - num2;
			}
			return double.NaN;
		}
	}
}
