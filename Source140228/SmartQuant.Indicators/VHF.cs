using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class VHF : Indicator
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
		public VHF(ISeries input, int length, BarData barData = BarData.Close) : base(input)
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
					"VHF (",
					this.length,
					", ",
					this.barData,
					")"
				});
			}
			else
			{
				this.name = "VHF (" + this.length + ")";
			}
			this.description = "Vertical Horizontal Filter";
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
			double num = VHF.Value(this.input, index, this.length, this.barData);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int length, BarData barData = BarData.Close)
		{
			if (index >= length)
			{
				double max = input.GetMax(index - length + 1, index, barData);
				double min = input.GetMin(index - length + 1, index, barData);
				double num = Math.Abs(max - min);
				double num2 = 0.0;
				for (int i = index; i > index - length; i--)
				{
					num2 += Math.Abs(input[i, barData] - input[i - 1, barData]);
				}
				return num / num2;
			}
			return double.NaN;
		}
	}
}
