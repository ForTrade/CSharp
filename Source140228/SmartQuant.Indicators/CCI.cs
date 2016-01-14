using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class CCI : Indicator
	{
		protected int length = 14;
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
		public CCI(ISeries input, int length) : base(input)
		{
			this.length = length;
			this.Init();
		}
		protected override void Init()
		{
			this.name = "CCI (" + this.length + ")";
			this.description = "Commodity Channel Index";
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
			double num = CCI.Value(this.input, index, this.length);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int length)
		{
			if (index >= length - 1)
			{
				double num = 0.0;
				for (int i = index - length + 1; i <= index; i++)
				{
					num += input[i, BarData.Typical];
				}
				num /= (double)length;
				double num2 = 0.0;
				for (int j = index - length + 1; j <= index; j++)
				{
					num2 += Math.Abs(input[j, BarData.Typical] - num);
				}
				num2 /= (double)length;
				return (input[index, BarData.Typical] - num) / (0.015 * num2);
			}
			return double.NaN;
		}
	}
}
