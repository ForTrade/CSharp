using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class ENVU : Indicator
	{
		protected int length;
		protected double shift;
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
		public new double Shift
		{
			get
			{
				return this.shift;
			}
			set
			{
				this.shift = value;
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
		public ENVU(ISeries input, int length, double shift, BarData barData = BarData.Close) : base(input)
		{
			this.length = length;
			this.shift = shift;
			this.barData = barData;
			this.Init();
		}
		protected override void Init()
		{
			if (this.input is BarSeries)
			{
				this.name = string.Concat(new object[]
				{
					"ENVU (",
					this.length,
					", ",
					this.shift,
					", ",
					this.barData,
					")"
				});
			}
			else
			{
				this.name = string.Concat(new object[]
				{
					"ENVU (",
					this.length,
					", ",
					this.shift,
					")"
				});
			}
			this.description = "Envelope Upper";
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
			double num = ENVU.Value(this.input, index, this.length, this.shift, this.barData);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int length, double shift, BarData barData = BarData.Close)
		{
			if (index >= length - 1)
			{
				double num = SMA.Value(input, index, length, barData);
				return num + num * (shift / 100.0);
			}
			return double.NaN;
		}
	}
}
