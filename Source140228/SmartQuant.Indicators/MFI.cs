using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class MFI : Indicator
	{
		protected int length;
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
		public MFI(ISeries input, int length) : base(input)
		{
			this.length = length;
			this.Init();
		}
		protected override void Init()
		{
			this.name = "MFI (" + this.length + ")";
			this.description = "Money Flow Index";
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
			double num = MFI.Value(this.input, index, this.length);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int length)
		{
			if (index >= length)
			{
				double num = 0.0;
				double num2 = 0.0;
				for (int i = index; i > index - length; i--)
				{
					double num3 = input[i, BarData.Typical];
					double num4 = input[i - 1, BarData.Typical];
					double num5 = input[i, BarData.Volume];
					double arg_48_0 = input[i - 1, BarData.Volume];
					if (num3 > num4)
					{
						num += num3 * num5;
					}
					else
					{
						num2 += num3 * num5;
					}
				}
				double num6 = num / num2;
				return 100.0 - 100.0 / (1.0 + num6);
			}
			return double.NaN;
		}
	}
}
