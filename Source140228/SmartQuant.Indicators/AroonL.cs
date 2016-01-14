using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class AroonL : Indicator
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
		public AroonL(ISeries input, int length) : base(input)
		{
			this.length = length;
			this.Init();
		}
		protected override void Init()
		{
			this.name = "AroonL (" + this.length + ")";
			this.description = "Aroon Lower";
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
			double num = AroonL.Value(this.input, index, this.length);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int length)
		{
			if (index >= length - 1)
			{
				double num = input[index, BarData.Low];
				double num2 = (double)index;
				for (int i = index - length + 1; i <= index; i++)
				{
					if (input[i, BarData.Low] < num)
					{
						num2 = (double)i;
						num = input[i, BarData.Low];
					}
				}
				double num3 = (double)index - num2;
				return 100.0 * (1.0 - num3 / (double)length);
			}
			return double.NaN;
		}
	}
}
