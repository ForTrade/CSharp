using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class CMO : Indicator
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
		public CMO(ISeries input, int length, BarData barData = BarData.Close) : base(input)
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
					"CMO (",
					this.length,
					", ",
					this.barData,
					")"
				});
			}
			else
			{
				this.name = "CMO (" + this.length + ")";
			}
			this.description = "Change Momentum Oscillator";
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
			double num = CMO.Value(this.input, index, this.length, this.barData);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int length, BarData barData = BarData.Close)
		{
			if (index >= length)
			{
				double num = 0.0;
				double num2 = 0.0;
				for (int i = index; i > index - length; i--)
				{
					double num3 = input[i, barData] - input[i - 1, barData];
					if (num3 > 0.0)
					{
						num += num3;
					}
					else
					{
						num2 -= num3;
					}
				}
				return 100.0 * (num - num2) / (num + num2);
			}
			return double.NaN;
		}
	}
}
