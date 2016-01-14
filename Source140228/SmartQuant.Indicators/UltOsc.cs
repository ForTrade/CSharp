using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class UltOsc : Indicator
	{
		protected int n1;
		protected int n2;
		protected int n3;
		[Category("Parameters"), Description("")]
		public int N1
		{
			get
			{
				return this.n1;
			}
			set
			{
				this.n1 = value;
				this.Init();
			}
		}
		[Category("Parameters"), Description("")]
		public int N2
		{
			get
			{
				return this.n2;
			}
			set
			{
				this.n2 = value;
				this.Init();
			}
		}
		[Category("Parameters"), Description("")]
		public int N3
		{
			get
			{
				return this.n3;
			}
			set
			{
				this.n3 = value;
				this.Init();
			}
		}
		public UltOsc(ISeries input, int n1, int n2, int n3) : base(input)
		{
			this.n1 = n1;
			this.n2 = n2;
			this.n3 = n3;
			this.Init();
		}
		protected override void Init()
		{
			this.name = string.Concat(new object[]
			{
				"UOSC (",
				this.n1,
				", ",
				this.n2,
				", ",
				this.n3,
				")"
			});
			this.description = "Ultimate Oscillator";
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
			double num = UltOsc.Value(this.input, index, this.n1, this.n2, this.n3);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int n1, int n2, int n3)
		{
			if (index >= Math.Max(n1, Math.Max(n2, n3)))
			{
				double num = 0.0;
				double num2 = 0.0;
				for (int i = index; i > index - n1; i--)
				{
					double num3 = input[i, BarData.Close];
					double val = input[i - 1, BarData.Close];
					double val2 = input[i, BarData.Low];
					num += num3 - Math.Min(val2, val);
					num2 += TR.Value(input, i);
				}
				double num4 = (double)(n3 / n1) * (num / num2);
				num = 0.0;
				num2 = 0.0;
				for (int j = index; j > index - n2; j--)
				{
					double num3 = input[j, BarData.Close];
					double val = input[j - 1, BarData.Close];
					double val2 = input[j, BarData.Low];
					num += num3 - Math.Min(val2, val);
					num2 += TR.Value(input, j);
				}
				double num5 = (double)(n3 / n2) * (num / num2);
				num = 0.0;
				num2 = 0.0;
				for (int k = index; k > index - n3; k--)
				{
					double num3 = input[k, BarData.Close];
					double val = input[k - 1, BarData.Close];
					double val2 = input[k, BarData.Low];
					num += num3 - Math.Min(val2, val);
					num2 += TR.Value(input, k);
				}
				double num6 = num / num2;
				return (num4 + num5 + num6) / (double)(n3 / n1 + n3 / n2 + 1) * 100.0;
			}
			return double.NaN;
		}
	}
}
