using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class FO : Indicator
	{
		protected int length;
		protected BarData barData;
		protected RegressionDistanceMode distanceMode;
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
		public FO(ISeries input, int length, BarData barData = BarData.Close, RegressionDistanceMode distanceMode = RegressionDistanceMode.Time) : base(input)
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
					"FO (",
					this.length,
					", ",
					this.barData,
					")"
				});
			}
			else
			{
				this.name = "FO (" + this.length + ")";
			}
			this.description = "Forecast Oscillator";
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
			double num = FO.Value(this.input, index, this.length, this.barData, this.distanceMode);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int length, BarData barData = BarData.Close, RegressionDistanceMode distanceMode = RegressionDistanceMode.Time)
		{
			if (index >= length - 1)
			{
				double num = input[index, barData];
				double num2 = LRI.Value(input, index, length, barData, distanceMode);
				return 100.0 * (num - num2) / num2;
			}
			return double.NaN;
		}
	}
}
