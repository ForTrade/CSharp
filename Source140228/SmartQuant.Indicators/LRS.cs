using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class LRS : Indicator
	{
		protected int length;
		protected BarData barData;
		private RegressionDistanceMode distanceMode;
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
		[Category("Parameters"), Description("")]
		public RegressionDistanceMode DistanceMode
		{
			get
			{
				return this.distanceMode;
			}
			set
			{
				this.distanceMode = value;
				this.Init();
			}
		}
		public LRS(ISeries input, int length, BarData barData = BarData.Close, RegressionDistanceMode distanceMode = RegressionDistanceMode.Time) : base(input)
		{
			this.length = length;
			this.barData = barData;
			this.distanceMode = distanceMode;
			this.Init();
		}
		protected override void Init()
		{
			if (this.input is BarSeries)
			{
				this.name = string.Concat(new object[]
				{
					"LRS (",
					this.length,
					", ",
					this.barData,
					")"
				});
			}
			else
			{
				this.name = "LRS (" + this.length + ")";
			}
			this.description = "Linear Regression Slope";
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
			double num = LRS.Value(this.input, index, this.length, this.barData, this.distanceMode);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int length, BarData barData = BarData.Close, RegressionDistanceMode distanceMode = RegressionDistanceMode.Time)
		{
			if (index >= length - 1)
			{
				double num = 0.0;
				double num2 = 0.0;
				double num3 = 0.0;
				double num4 = 0.0;
				if (distanceMode == RegressionDistanceMode.Time)
				{
					double num5 = (double)input.GetDateTime(index).Subtract(input.GetDateTime(index - 1)).Ticks;
					for (int i = index; i > index - length; i--)
					{
						num += (double)input.GetDateTime(i).Subtract(input.GetDateTime(index - length + 1)).Ticks / num5;
						num2 += (double)input.GetDateTime(i).Subtract(input.GetDateTime(index - length + 1)).Ticks / num5 * input[i, barData];
						num3 += input[i, barData];
						num4 += (double)input.GetDateTime(i).Subtract(input.GetDateTime(index - length + 1)).Ticks / num5 * (double)input.GetDateTime(i).Subtract(input.GetDateTime(index - length + 1)).Ticks / num5;
					}
				}
				else
				{
					for (int j = index; j > index - length; j--)
					{
						num += (double)(j - index + length - 1);
						num2 += (double)(j - index + length - 1) * input[j, barData];
						num3 += input[j, barData];
						num4 += (double)((j - index + length - 1) * (j - index + length - 1));
					}
				}
				return ((double)length * num2 - num * num3) / ((double)length * num4 - Math.Pow(num, 2.0));
			}
			return double.NaN;
		}
	}
}
