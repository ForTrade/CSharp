using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class PCL : Indicator
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
		public PCL(ISeries input, int length) : base(input)
		{
			this.length = length;
			this.Init();
		}
		protected override void Init()
		{
			this.name = "PCL (" + this.length + ")";
			this.description = "Price Channel Lower";
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
			double num = PCL.Value(this.input, index, this.length);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int length)
		{
			if (index >= length)
			{
				return input.GetMin(index - length, index - 1, BarData.Low);
			}
			return double.NaN;
		}
	}
}
