using System;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class NVI : Indicator
	{
		public NVI(ISeries input) : base(input)
		{
			this.Init();
		}
		protected override void Init()
		{
			this.name = "NVI";
			this.description = "Negative Volume Index";
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
			if (index >= 1)
			{
				double num = this.input[index, BarData.Close];
				double num2 = this.input[index - 1, BarData.Close];
				double num3 = this.input[index, BarData.Volume];
				double num4 = this.input[index - 1, BarData.Volume];
				double num5 = this[index - 1];
				double num6;
				if (num3 < num4)
				{
					num6 = num5 + num5 * (num - num2) / num2;
				}
				else
				{
					num6 = num5;
				}
				if (!double.IsNaN(num6))
				{
					base.Add(this.input.GetDateTime(index), num6);
					return;
				}
			}
			else
			{
				if (index == 0)
				{
					double value = this.input[0, BarData.Volume];
					base.Add(this.input.GetDateTime(index), value);
				}
			}
		}
		public static double Value(ISeries input, int index)
		{
			if (index >= 1)
			{
				double num = input[index, BarData.Close];
				double num2 = input[index - 1, BarData.Close];
				double num3 = input[index, BarData.Volume];
				double num4 = input[index - 1, BarData.Volume];
				double num5 = NVI.Value(input, index - 1);
				double result;
				if (num3 < num4)
				{
					result = num5 + num5 * (num - num2) / num2;
				}
				else
				{
					result = num5;
				}
				return result;
			}
			if (index == 0)
			{
				return input[0, BarData.Volume];
			}
			return double.NaN;
		}
	}
}
