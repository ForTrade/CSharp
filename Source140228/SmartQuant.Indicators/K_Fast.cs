using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class K_Fast : Indicator
	{
		protected int length;
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
		public K_Fast(ISeries input, int length) : base(input)
		{
			this.length = length;
			this.Init();
		}
		protected override void Init()
		{
			this.name = "%K Fast (" + this.length + ")";
			this.description = "%K Fast";
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
			double num = K_Fast.Value(this.input, index, this.length);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int length)
		{
			if (index >= length - 1)
			{
				double num = input[index, BarData.Close];
				double min = input.GetMin(index - length + 1, index, BarData.Low);
				double max = input.GetMax(index - length + 1, index, BarData.High);
				return 100.0 * (num - min) / (max - min);
			}
			return double.NaN;
		}
	}
}
