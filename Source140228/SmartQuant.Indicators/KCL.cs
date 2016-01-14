using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class KCL : Indicator
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
		public KCL(ISeries input, int length) : base(input)
		{
			this.length = length;
			this.Init();
		}
		protected override void Init()
		{
			this.name = "KCL (" + this.length + ")";
			this.description = "Keltner Channel Lower";
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
			double num = KCL.Value(this.input, index, this.length);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int length)
		{
			if (index >= length)
			{
				double num = 0.0;
				for (int i = index - length + 1; i <= index; i++)
				{
					num += TR.Value(input, i);
				}
				num /= (double)length;
				return SMA.Value(input, index, length, BarData.Typical) - num;
			}
			return double.NaN;
		}
	}
}
