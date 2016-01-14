using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class CAD : Indicator
	{
		protected AD ad;
		protected EMA ema1;
		protected EMA ema2;
		protected int length1;
		protected int length2;
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
		public CAD(ISeries input, int length1, int length2) : base(input)
		{
			this.length1 = length1;
			this.length2 = length2;
			this.Init();
		}
		protected override void Init()
		{
			this.name = string.Concat(new object[]
			{
				"CAD (",
				this.length1,
				", ",
				this.length2,
				")"
			});
			this.description = "Chaikin A/D Oscillator";
			base.Clear();
			this.calculate = true;
			base.Detach();
			if (this.ad != null)
			{
				this.ad.Detach();
			}
			if (this.ema1 != null)
			{
				this.ema1.Detach();
			}
			if (this.ema2 != null)
			{
				this.ema2.Detach();
			}
			this.ad = new AD(this.input);
			this.ema1 = new EMA(this.ad, this.length1, BarData.Close);
			this.ema2 = new EMA(this.ad, this.length2, BarData.Close);
			base.Attach();
		}
		protected internal override void Calculate(int index)
		{
			if (this.calculate)
			{
				this.Calculate();
				return;
			}
			if (index >= Math.Max(this.length1, this.length2))
			{
				double value = this.ema1[index] - this.ema2[index];
				base.Add(this.input.GetDateTime(index), value);
			}
		}
		public static double Value(ISeries input, int index, int length1, int length2)
		{
			if (index >= Math.Max(length1, length2))
			{
				AD input2 = new AD(input);
				EMA eMA = new EMA(input2, length1, BarData.Close);
				EMA eMA2 = new EMA(input2, length2, BarData.Close);
				return eMA[index] - eMA2[index];
			}
			return double.NaN;
		}
	}
}
