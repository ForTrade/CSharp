using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class ADXR : Indicator
	{
		protected IndicatorStyle style;
		protected int length;
		protected ADX adx;
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
		public ADXR(ISeries input, int length, IndicatorStyle style = IndicatorStyle.QuantStudio) : base(input)
		{
			this.length = length;
			this.style = style;
			this.Init();
		}
		protected override void Init()
		{
			this.name = "ADXR (" + this.length + ")";
			this.description = "Average Directional Index Rating";
			base.Clear();
			this.calculate = true;
			base.Detach();
			this.adx = new ADX(this.input, this.length, this.style);
			base.Attach();
		}
		protected internal override void Calculate(int index)
		{
			if (this.calculate)
			{
				this.Calculate();
				return;
			}
			if (index >= 3 * this.length - 1)
			{
				int num = -2 * this.length;
				double num2 = this.adx[index + num];
				double num3 = this.adx[index - this.length + 1 + num];
				double value = (num2 + num3) / 2.0;
				base.Add(this.input.GetDateTime(index), value);
			}
		}
		public static double Value(ISeries input, int index, int length, IndicatorStyle style = IndicatorStyle.QuantStudio)
		{
			if (index >= 3 * length - 1)
			{
				double num = ADX.Value(input, index, length, style);
				double num2 = ADX.Value(input, index - length + 1, length, style);
				return (num + num2) / 2.0;
			}
			return double.NaN;
		}
	}
}
