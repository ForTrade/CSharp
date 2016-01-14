using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class K_Slow : Indicator
	{
		protected int length;
		protected int order;
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
		public K_Slow(ISeries input, int length, int order) : base(input)
		{
			this.length = length;
			this.order = order;
			this.Init();
		}
		protected override void Init()
		{
			this.name = string.Concat(new object[]
			{
				"%K Slow (",
				this.length,
				", ",
				this.order,
				")"
			});
			this.description = "%K SLow";
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
			double num = K_Slow.Value(this.input, index, this.length, this.order);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int length, int order)
		{
			if (index >= length + order - 1)
			{
				double num = 0.0;
				for (int i = index; i > index - order; i--)
				{
					double min = input.GetMin(i - length + 1, i, BarData.Low);
					double max = input.GetMax(i - length + 1, i, BarData.High);
					double num2 = input[i, BarData.Close];
					double num3 = max - min;
					double num4 = num2 - min;
					num += 100.0 * num4 / num3;
				}
				return num / (double)order;
			}
			return double.NaN;
		}
	}
}
