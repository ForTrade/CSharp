using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class MDI : Indicator
	{
		protected IndicatorStyle style;
		protected int length;
		protected TimeSeries mdmTS;
		protected TimeSeries trTS;
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
		public MDI(ISeries input, int length, IndicatorStyle style = IndicatorStyle.QuantStudio) : base(input)
		{
			this.length = length;
			this.style = style;
			this.Init();
		}
		protected override void Init()
		{
			this.name = "MDI (" + this.length + ")";
			this.description = "Minus Directional Indicator";
			base.Clear();
			this.calculate = true;
			this.mdmTS = new TimeSeries();
			this.trTS = new TimeSeries();
		}
		protected internal override void Calculate(int index)
		{
			if (this.calculate)
			{
				this.Calculate();
				return;
			}
			if (this.style == IndicatorStyle.QuantStudio)
			{
				double num = 0.0;
				double num2 = 0.0;
				if (index >= this.length)
				{
					if (index == this.length)
					{
						for (int i = index; i >= index - this.length + 1; i--)
						{
							num2 += TR.Value(this.input, i);
							num += MDM.Value(this.input, i);
						}
					}
					else
					{
						num = this.mdmTS[index - 1] - MDM.Value(this.input, index - this.length) + MDM.Value(this.input, index);
						num2 = this.trTS[index - 1] - TR.Value(this.input, index - this.length) + TR.Value(this.input, index);
					}
					if (num2 != 0.0)
					{
						double num3 = num / num2 * 100.0;
						if (!double.IsNaN(num3))
						{
							base.Add(this.input.GetDateTime(index), num3);
						}
					}
				}
				this.mdmTS.Add(this.input.GetDateTime(index), num);
				this.trTS.Add(this.input.GetDateTime(index), num2);
				return;
			}
			double num4 = 0.0;
			double num5 = 0.0;
			if (index >= this.length)
			{
				if (index == this.length)
				{
					for (int j = index; j >= index - this.length + 1; j--)
					{
						num5 += TR.Value(this.input, j);
						num4 += MDM.Value(this.input, j);
					}
				}
				else
				{
					num4 = this.mdmTS[index - 1] - this.mdmTS[index - 1] / (double)this.length + MDM.Value(this.input, index);
					num5 = this.trTS[index - 1] - this.trTS[index - 1] / (double)this.length + TR.Value(this.input, index);
				}
				if (num5 != 0.0)
				{
					double num6 = num4 / num5 * 100.0;
					if (!double.IsNaN(num6))
					{
						base.Add(this.input.GetDateTime(index), num6);
					}
				}
			}
			this.mdmTS.Add(this.input.GetDateTime(index), num4);
			this.trTS.Add(this.input.GetDateTime(index), num5);
		}
		public static double Value(ISeries input, int index, int length, IndicatorStyle style = IndicatorStyle.QuantStudio)
		{
			if (style == IndicatorStyle.QuantStudio)
			{
				double num = 0.0;
				double num2 = 0.0;
				if (index >= length)
				{
					for (int i = index; i > index - length; i--)
					{
						num2 += TR.Value(input, i);
						num += MDM.Value(input, i);
					}
					return num / num2 * 100.0;
				}
				return double.NaN;
			}
			else
			{
				double num3 = 0.0;
				double num4 = 0.0;
				if (index >= length)
				{
					for (int j = length; j >= 1; j--)
					{
						num4 += TR.Value(input, j);
						num3 += MDM.Value(input, j);
					}
					for (int k = length + 1; k <= index; k++)
					{
						num3 = num3 - num3 / (double)length + MDM.Value(input, k);
						num4 = num4 - num4 / (double)length + TR.Value(input, k);
					}
					return num3 / num4 * 100.0;
				}
				return double.NaN;
			}
		}
	}
}
