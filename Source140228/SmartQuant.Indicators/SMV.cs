using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class SMV : Indicator
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
		public SMV(ISeries input, int length, BarData barData = BarData.Close) : base(input)
		{
			this.length = length;
			this.barData = barData;
			this.Init();
		}
		protected override void Init()
		{
			this.name = string.Concat(new object[]
			{
				"SMV (",
				this.length,
				", ",
				this.barData,
				")"
			});
			this.description = "Simple Moving Variance";
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
			double num = SMV.Value(this.input, index, this.length, this.barData);
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
				double num2 = SMA.Value(input, index, length, barData);
				for (int i = index; i > index - length; i--)
				{
					num += (num2 - input[i, barData]) * (num2 - input[i, barData]);
				}
				return num / (double)length;
			}
			return double.NaN;
		}
	}
}
