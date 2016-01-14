using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class VCH : Indicator
	{
		protected int length1;
		protected int length2;
		protected EMA ema;
		protected TimeSeries hlTS;
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
		public VCH(ISeries input, int length1, int length2) : base(input)
		{
			this.length1 = length1;
			this.length2 = length2;
			this.Init();
		}
		protected override void Init()
		{
			this.name = string.Concat(new object[]
			{
				"VCH (",
				this.length1,
				", ",
				this.length2,
				")"
			});
			this.description = "Chaikin Volatility";
			base.Clear();
			this.calculate = true;
			this.hlTS = new TimeSeries();
			for (int i = 0; i < this.input.Count; i++)
			{
				this.hlTS.Add(this.input.GetDateTime(i), this.input[i, BarData.High] - this.input[i, BarData.Low]);
			}
			this.ema = new EMA(this.hlTS, this.length1, BarData.Close);
		}
		protected internal override void Calculate(int index)
		{
			if (this.calculate)
			{
				this.Calculate();
				return;
			}
			this.hlTS.Add(this.input.GetDateTime(index), this.input[index, BarData.High] - this.input[index, BarData.Low]);
			if (index >= this.length2 - 1)
			{
				int index2 = this.ema.GetIndex(this.input.GetDateTime(index), IndexOption.Null);
				double num = (this.ema[index2] - this.ema[index2 - this.length2 + 1]) / this.ema[index2 - this.length2 + 1] * 100.0;
				if (!double.IsNaN(num))
				{
					base.Add(this.input.GetDateTime(index), num);
				}
			}
		}
		public static double Value(ISeries input, int index, int length1, int length2)
		{
			if (index >= length2 - 1)
			{
				TimeSeries timeSeries = new TimeSeries();
				for (int i = 0; i <= index; i++)
				{
					timeSeries.Add(input.GetDateTime(i), input[i, BarData.High] - input[i, BarData.Low]);
				}
				EMA eMA = new EMA(timeSeries, length1, BarData.Close);
				return (eMA[index] - eMA[index - length2 + 1]) / eMA[index - length2 + 1] * 100.0;
			}
			return double.NaN;
		}
	}
}
