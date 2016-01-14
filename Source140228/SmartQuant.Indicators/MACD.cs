using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class MACD : Indicator
	{
		protected int length1;
		protected int length2;
		protected BarData barData;
		protected EMA ema1;
		protected EMA ema2;
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
		public MACD(ISeries input, int length1, int length2, BarData barData = BarData.Close) : base(input)
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
					"MACD (",
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
					"MACD (",
					this.length1,
					", ",
					this.length2,
					")"
				});
			}
			this.description = "Moving Average Convergence Divergence";
			base.Clear();
			this.calculate = true;
			base.Detach();
			this.ema1 = new EMA(this.input, this.length1, this.barData);
			this.ema2 = new EMA(this.input, this.length2, this.barData);
			base.Attach();
		}
		protected internal override void Calculate(int index)
		{
			if (this.calculate)
			{
				this.Calculate();
				return;
			}
			if (index >= 0)
			{
				double num = this.ema1[index];
				double num2 = this.ema2[index];
				double value = num - num2;
				base.Add(this.input.GetDateTime(index), value);
			}
		}
		public static double Value(ISeries input, int index, int length1, int length2, BarData barData = BarData.Close)
		{
			if (index >= 0)
			{
				double num = EMA.Value(input, index, length1, barData);
				double num2 = EMA.Value(input, index, length2, barData);
				return num - num2;
			}
			return double.NaN;
		}
	}
}
