using System;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class AD : Indicator
	{
		public AD(ISeries input) : base(input)
		{
			this.Init();
		}
		protected override void Init()
		{
			this.name = "AD ";
			this.description = "Accumulation/Distribution";
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
			double num = this.input[index, BarData.High];
			double num2 = this.input[index, BarData.Low];
			double num3 = this.input[index, BarData.Close];
			double arg_46_0 = this.input[index, BarData.Open];
			double num4 = this.input[index, BarData.Volume];
			double num5 = double.NaN;
			if (index >= 1)
			{
				if (num != num2)
				{
					num5 = num4 * (num3 - num2 - (num - num3)) / (num - num2) + this[index - 1];
				}
				else
				{
					num5 = this[index - 1 - 1];
				}
			}
			else
			{
				if (index == 0 && num != num2)
				{
					num5 = num4 * (num3 - num2 - (num - num3)) / (num - num2);
				}
			}
			if (!double.IsNaN(num5))
			{
				base.Add(this.input.GetDateTime(index), num5);
			}
		}
		public static double Value(ISeries input, int index)
		{
			if (index >= 0)
			{
				double num = input[index, BarData.High];
				double num2 = input[index, BarData.Low];
				double num3 = input[index, BarData.Close];
				double arg_2A_0 = input[index, BarData.Open];
				double num4 = input[index, BarData.Volume];
				double result = 0.0;
				if (index >= 1)
				{
					if (num != num2)
					{
						result = num4 * (num3 - num2 - (num - num3)) / (num - num2) + AD.Value(input, index - 1);
					}
					else
					{
						result = AD.Value(input, index - 1);
					}
				}
				else
				{
					if (index == 0 && num != num2)
					{
						result = num4 * (num3 - num2 - (num - num3)) / (num - num2);
					}
				}
				return result;
			}
			return double.NaN;
		}
	}
}
