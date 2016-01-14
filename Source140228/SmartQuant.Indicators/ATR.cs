using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class ATR : Indicator
	{
		protected IndicatorStyle style;
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
		[Category("Parameters"), Description("")]
		public IndicatorStyle Style
		{
			get
			{
				return this.style;
			}
			set
			{
				this.style = value;
				this.Init();
			}
		}
		public ATR(ISeries input, int length, IndicatorStyle style = IndicatorStyle.QuantStudio) : base(input)
		{
			this.length = length;
			this.style = style;
			this.Init();
		}
		protected override void Init()
		{
			this.name = "ATR (" + this.length + ")";
			this.description = "Average True Range";
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
			if (index >= this.length)
			{
				int num = -1 * this.length;
				double value;
				if (this.style == IndicatorStyle.QuantStudio)
				{
					if (index == this.length)
					{
						double num2 = 0.0;
						for (int i = index; i > index - this.length; i--)
						{
							num2 += TR.Value(this.input, i);
						}
						value = num2 / (double)this.length;
					}
					else
					{
						double num3 = TR.Value(this.input, index);
						value = (this[index - 1 + num] * (double)this.length + num3 - TR.Value(this.input, index - this.length)) / (double)this.length;
					}
				}
				else
				{
					if (index == this.length)
					{
						double num4 = 0.0;
						for (int j = index; j > index - this.length; j--)
						{
							num4 += TR.Value(this.input, j);
						}
						value = num4 / (double)this.length;
					}
					else
					{
						double num3 = TR.Value(this.input, index);
						value = (base[this.input.GetDateTime(index - 1), SearchOption.Exact] * (double)this.length + num3 - TR.Value(this.input, index - this.length)) / (double)this.length;
					}
				}
				base.Add(this.input.GetDateTime(index), value);
			}
		}
		public static double Value(ISeries input, int index, int length, IndicatorStyle style = IndicatorStyle.QuantStudio)
		{
			if (index >= length)
			{
				double num = 0.0;
				double result;
				if (style == IndicatorStyle.QuantStudio)
				{
					for (int i = index; i > index - length; i--)
					{
						num += TR.Value(input, i);
					}
					result = num / (double)length;
				}
				else
				{
					for (int j = length; j > 0; j--)
					{
						num += TR.Value(input, j);
					}
					num /= (double)length;
					for (int k = length + 1; k <= index; k++)
					{
						num = (num * (double)(length - 1) + TR.Value(input, k)) / (double)length;
					}
					result = num;
				}
				return result;
			}
			return double.NaN;
		}
	}
}
