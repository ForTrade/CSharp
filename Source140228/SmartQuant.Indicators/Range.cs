using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class Range : Indicator
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
		public Range(ISeries input, int length) : base(input)
		{
			this.length = length;
			this.Init();
		}
		protected override void Init()
		{
			this.name = "Range (" + this.length + ")";
			this.description = "Range";
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
			double num = Range.Value(this.input, index, this.length);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int length)
		{
			if (index >= length - 1)
			{
				double min = input.GetMin(index - length + 1, index, BarData.Low);
				double max = input.GetMax(index - length + 1, index, BarData.High);
				return Math.Log(max / min);
			}
			return double.NaN;
		}
	}
}
