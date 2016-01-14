using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class PERF : Indicator
	{
		protected double k;
		protected BarData barData;
		[Category("Parameters"), Description("")]
		public double K
		{
			get
			{
				return this.k;
			}
			set
			{
				this.k = value;
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
		public PERF(ISeries input, double k, BarData barData = BarData.Close) : base(input)
		{
			this.k = k;
			this.barData = barData;
			this.Init();
		}
		protected override void Init()
		{
			if (this.input is BarSeries)
			{
				this.name = string.Concat(new object[]
				{
					"PERF (",
					this.k,
					", ",
					this.barData,
					")"
				});
			}
			else
			{
				this.name = "PERF (" + this.k + ")";
			}
			this.description = "Performance Indicator";
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
			double num = PERF.Value(this.input, index, this.k, this.barData);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, double k, BarData barData = BarData.Close)
		{
			if (index >= 0)
			{
				double num = input[index, barData];
				return 100.0 * (num - k) / k;
			}
			return double.NaN;
		}
	}
}
