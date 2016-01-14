using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class SAR : Indicator
	{
		private double upperBound;
		private double step;
		private double initialAcc;
		private double lastSAR;
		private double sar;
		private double sip;
		private double acc;
		private double diff;
		private double prevLow;
		private double prevHigh;
		private double minClose;
		private int barsCount;
		private bool isLong;
		[Category("Parameters"), Description("The maximum possible value of the Acceleration Factor")]
		public double UpperBound
		{
			get
			{
				return this.upperBound;
			}
			set
			{
				this.upperBound = value;
				this.Init();
			}
		}
		[Category("Parameters"), Description("Step that is used to increment the Acceleration Factor")]
		public double Step
		{
			get
			{
				return this.step;
			}
			set
			{
				this.step = value;
				this.Init();
			}
		}
		[Category("Parameters"), Description("Initial value of the Acceleration Factor")]
		public double InitialAcc
		{
			get
			{
				return this.initialAcc;
			}
			set
			{
				this.initialAcc = value;
				this.Init();
			}
		}
		public SAR(ISeries input, double upperBound, double step, double initialAcc) : base(input)
		{
			this.upperBound = upperBound;
			this.step = step;
			this.initialAcc = initialAcc;
			this.Init();
		}
		protected override void Init()
		{
			this.name = string.Concat(new object[]
			{
				"SAR (",
				this.upperBound,
				", ",
				this.step,
				", ",
				this.initialAcc,
				")"
			});
			this.description = "Parabolic SAR";
			base.Clear();
			this.ResetSAR();
			this.calculate = true;
		}
		private void ResetSAR()
		{
			this.lastSAR = 0.0;
			this.sar = 0.0;
			this.sip = 0.0;
			this.acc = 0.0;
			this.diff = 0.0;
			this.prevLow = 0.0;
			this.prevHigh = 0.0;
			this.minClose = 1.7976931348623157E+308;
			this.barsCount = 0;
			this.isLong = false;
		}
		protected internal override void Calculate(int index)
		{
			if (this.calculate)
			{
				this.Calculate();
				return;
			}
			BarSeries barSeries = this.input as BarSeries;
			Bar bar = barSeries[index];
			this.barsCount++;
			if (this.barsCount > 20)
			{
				this.lastSAR = this.sar;
				if (this.isLong)
				{
					if (bar.High > this.sip)
					{
						this.sip = bar.High;
						if (this.acc < this.upperBound)
						{
							this.acc += this.step;
						}
					}
					if (bar.Low < this.sar)
					{
						this.isLong = false;
					}
					if (this.isLong)
					{
						this.diff = this.sip - this.sar;
						this.sar += this.diff * this.acc;
						if (this.sar > bar.Low || this.sar > this.prevLow)
						{
							if (bar.Low < this.prevLow)
							{
								this.sar = bar.Low;
							}
							else
							{
								this.sar = this.prevLow;
							}
						}
						if (this.sar < this.lastSAR)
						{
							this.sar = this.lastSAR;
						}
						if (!double.IsNaN(this.sar))
						{
							base.Add(this.input.GetDateTime(index), this.sar);
						}
					}
					else
					{
						this.sar = this.sip;
						this.acc = this.initialAcc;
						if (!double.IsNaN(this.sar))
						{
							base.Add(this.input.GetDateTime(index), this.sar);
						}
					}
				}
				else
				{
					if (this.prevLow < this.sip)
					{
						this.sip = this.prevLow;
						if (this.acc < this.upperBound)
						{
							this.acc += this.step;
						}
					}
					if (bar.High > this.sar)
					{
						this.isLong = true;
					}
					if (!this.isLong)
					{
						this.diff = this.sar - this.sip;
						this.sar -= this.diff * this.acc;
						if (this.sar < bar.High || this.sar < this.prevHigh)
						{
							if (bar.High > this.prevHigh)
							{
								this.sar = bar.High;
							}
							else
							{
								this.sar = this.prevHigh;
							}
						}
						if (this.sar > this.lastSAR)
						{
							this.sar = this.lastSAR;
						}
						if (!double.IsNaN(this.sar))
						{
							base.Add(this.input.GetDateTime(index), this.sar);
						}
					}
					else
					{
						this.sar = this.sip;
						this.acc = this.initialAcc;
						if (!double.IsNaN(this.sar))
						{
							base.Add(this.input.GetDateTime(index), this.sar);
						}
					}
				}
			}
			else
			{
				if (this.barsCount == 20)
				{
					this.isLong = true;
					this.sip = this.minClose;
					this.sar = this.sip;
				}
				else
				{
					this.minClose = Math.Min(this.minClose, bar.Close);
				}
			}
			this.prevHigh = bar.High;
			this.prevLow = bar.Low;
		}
		public static double Value(ISeries input, int index, double upperBound, double step, double initialAcc)
		{
			if (index >= 0)
			{
				return new SAR(input, upperBound, step, initialAcc)[index];
			}
			return double.NaN;
		}
	}
}
