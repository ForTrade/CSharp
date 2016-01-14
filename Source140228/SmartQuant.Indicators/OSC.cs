using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class OSC : Indicator
	{
		protected int length1;
		protected int length2;
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
		public int Length1
		{
			get
			{
				return this.length1;
			}
			set
			{
				this.length1 = value;
				this.Init();
			}
		}
		[Category("Parameters"), Description("")]
		public int Length2
		{
			get
			{
				return this.length2;
			}
			set
			{
				this.length2 = value;
				this.Init();
			}
		}
		public OSC(ISeries input, int length1, int length2, BarData barData = BarData.Close) : base(input)
		{
			this.length1 = length1;
			this.length2 = length2;
			this.barData = barData;
			this.Init();
		}
		protected override void Init()
		{
			if (this.input is BarSeries)
			{
				this.name = string.Concat(new object[]
				{
					"OSC (",
					this.length1,
					", ",
					this.length2,
					", ",
					this.barData,
					")"
				});
			}
			else
			{
				this.name = string.Concat(new object[]
				{
					"OSC (",
					this.length1,
					", ",
					this.length2,
					")"
				});
			}
			this.description = "Oscillator";
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
			double num = OSC.Value(this.input, index, this.length1, this.length2, this.barData);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int length1, int length2, BarData barData = BarData.Close)
		{
			if (index >= length1 - 1 && index >= length2 - 1)
			{
				double num = SMA.Value(input, index, length1, barData);
				double num2 = SMA.Value(input, index, length2, barData);
				return num - num2;
			}
			return double.NaN;
		}
	}
}
