using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class MASS : Indicator
	{
		protected int length;
		protected int order;
		protected EMA ema;
		protected EMA ema_ema;
		protected TimeSeries hlTS;
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
		public int Order
		{
			get
			{
				return this.order;
			}
			set
			{
				this.order = value;
				this.Init();
			}
		}
		public MASS(ISeries input, int length, int order) : base(input)
		{
			this.length = length;
			this.order = order;
			this.Init();
		}
		protected override void Init()
		{
			this.name = string.Concat(new object[]
			{
				"MASS (",
				this.length,
				", ",
				this.order,
				")"
			});
			this.description = "Mass Index";
			base.Clear();
			this.calculate = true;
			this.hlTS = new TimeSeries();
			for (int i = 0; i < this.input.Count; i++)
			{
				this.hlTS.Add(this.input.GetDateTime(i), this.input[i, BarData.High] - this.input[i, BarData.Low]);
			}
			base.Detach();
			this.ema = new EMA(this.hlTS, this.order, BarData.Close);
			this.ema_ema = new EMA(this.ema, this.order, BarData.Close);
			base.Attach();
		}
		protected internal override void Calculate(int index)
		{
			if (this.calculate)
			{
				this.Calculate();
				return;
			}
			this.hlTS.Add(this.input.GetDateTime(index), this.input[index, BarData.High] - this.input[index, BarData.Low]);
			if (index >= this.length - 1)
			{
				double num = 0.0;
				for (int i = index; i > index - this.length; i--)
				{
					num += this.ema[i] / this.ema_ema[i];
				}
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int length, int order)
		{
			if (index >= length - 1)
			{
				TimeSeries timeSeries = new TimeSeries();
				for (int i = 0; i <= index; i++)
				{
					timeSeries.Add(input.GetDateTime(i), input[i, BarData.High] - input[i, BarData.Low]);
				}
				EMA eMA = new EMA(timeSeries, order, BarData.Close);
				EMA eMA2 = new EMA(eMA, order, BarData.Close);
				double num = 0.0;
				for (int j = index; j > index - length; j--)
				{
					num += eMA[j] / eMA2[j];
				}
				return num;
			}
			return double.NaN;
		}
	}
}
