using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	public class HV : Indicator
	{
		protected int length;
		protected double span;
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
		public double Span
		{
			get
			{
				return this.span;
			}
			set
			{
				this.span = value;
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
		public HV(ISeries input, int length, double span, BarData barData = BarData.Close) : base(input)
		{
			this.length = length;
			this.span = span;
			this.barData = barData;
			this.Init();
		}
		protected override void Init()
		{
			if (this.input is BarSeries)
			{
				this.name = string.Concat(new object[]
				{
					"HV (",
					this.length,
					", ",
					this.span,
					", ",
					this.barData,
					")"
				});
			}
			else
			{
				this.name = string.Concat(new object[]
				{
					"HV (",
					this.length,
					", ",
					this.span,
					")"
				});
			}
			this.description = "Historical Volatility";
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
			double num = HV.Value(this.input, index, this.length, this.span, this.barData);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int length, double span, BarData barData = BarData.Close)
		{
			if (index >= length)
			{
				double[] array = new double[length];
				double num = 0.0;
				for (int i = index; i > index - length; i--)
				{
					double d = input[i, barData] / input[i - 1, barData];
					double num2 = Math.Log(d);
					num += num2;
					array[i - index + length - 1] = num2;
				}
				double num3 = num / (double)length;
				double num4 = 0.0;
				for (int j = 0; j < array.Length; j++)
				{
					num4 += Math.Pow(array[j] - num3, 2.0);
				}
				num4 /= (double)(length - 1);
				return Math.Sqrt(num4) * 100.0 * Math.Sqrt(span);
			}
			return double.NaN;
		}
	}
}
