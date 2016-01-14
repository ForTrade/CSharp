using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class VOSC : Indicator
	{
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
		public VOSC(ISeries input, int length1, int length2) : base(input)
		{
			this.length1 = length1;
			this.length2 = length2;
			this.Init();
		}
		protected override void Init()
		{
			this.name = string.Concat(new object[]
			{
				"VOSC (",
				this.length1,
				", ",
				this.length2,
				")"
			});
			this.description = "Volume Oscillator";
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
			double num = VOSC.Value(this.input, index, this.length1, this.length2);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int length1, int length2)
		{
			if (index >= length1 - 1 && index >= length2 - 1)
			{
				TimeSeries timeSeries = new TimeSeries();
				for (int i = index - Math.Max(length1, length2) + 1; i <= index; i++)
				{
					timeSeries.Add(input.GetDateTime(i), input[i, BarData.Volume]);
				}
				double num = SMA.Value(timeSeries, length1 - 1, length1, BarData.Close);
				double num2 = SMA.Value(timeSeries, length2 - 1, length2, BarData.Close);
				return num - num2;
			}
			return double.NaN;
		}
	}
}
