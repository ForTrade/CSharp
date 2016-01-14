using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class TRIX : Indicator
	{
		protected int length;
		protected BarData barData;
		protected EMA ema1;
		protected EMA ema2;
		protected EMA ema3;
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
		public TRIX(ISeries input, int length, BarData barData = BarData.Close) : base(input)
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
					"TRIX (",
					this.length,
					", ",
					this.barData,
					")"
				});
			}
			else
			{
				this.name = "TRIX (" + this.length + " )";
			}
			this.description = "TRIX Index";
			base.Clear();
			this.calculate = true;
			base.Detach();
			this.ema1 = new EMA(this.input, this.length, this.barData);
			base.Attach();
			this.ema2 = new EMA(this.ema1, this.length, this.barData);
			this.ema3 = new EMA(this.ema2, this.length, this.barData);
		}
		protected internal override void Calculate(int index)
		{
			if (this.calculate)
			{
				this.Calculate();
				return;
			}
			if (index >= 1)
			{
				double num = (this.ema3[index] - this.ema3[index - 1]) / this.ema3[index - 1] * 100.0;
				if (!double.IsNaN(num))
				{
					base.Add(this.input.GetDateTime(index), num);
				}
			}
		}
		public static double Value(ISeries input, int index, int length, BarData barData = BarData.Close)
		{
			if (index >= 1)
			{
				TimeSeries timeSeries = new TimeSeries();
				for (int i = 0; i <= index; i++)
				{
					timeSeries.Add(input.GetDateTime(i), input[i, barData]);
				}
				EMA input2 = new EMA(timeSeries, length, barData);
				EMA input3 = new EMA(input2, length, barData);
				EMA eMA = new EMA(input3, length, barData);
				return (eMA[index] - eMA[index - 1]) / eMA[index - 1] * 100.0;
			}
			return double.NaN;
		}
	}
}
