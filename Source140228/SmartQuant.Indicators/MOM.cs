using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class MOM : Indicator
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
		public MOM(ISeries input, int length, BarData barData = BarData.Close) : base(input)
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
					"MOM (",
					this.length,
					", ",
					this.barData,
					")"
				});
			}
			else
			{
				this.name = "MOM (" + this.length + ")";
			}
			this.description = "Momentum Oscillator";
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
			double num = MOM.Value(this.input, index, this.length, this.barData);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int length, BarData barData = BarData.Close)
		{
			if (index >= length - 1)
			{
				return input[index, barData] - input[index - length + 1, barData];
			}
			return double.NaN;
		}
	}
}
