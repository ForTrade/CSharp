using System;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class OBV : Indicator
	{
		public OBV(ISeries input) : base(input)
		{
			this.Init();
		}
		protected override void Init()
		{
			this.name = "OBV";
			this.description = "On Balance Volume";
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
				int num4 = -1;
				double num5 = 0.0;
				if (index > 1)
				{
					if (num > num2)
					{
						num5 = this[index - 1 + num4] + num3;
					}
					if (num < num2)
					{
						num5 = this[index - 1 + num4] - num3;
					}
					if (num == num2)
					{
						num5 = this[index - 1 + num4];
					}
				}
				else
				{
					if (num > num2)
					{
						num5 = num3;
					}
					if (num < num2)
					{
						num5 = -num3;
					}
					if (num == num2)
					{
						num5 = 0.0;
					}
				}
				if (!double.IsNaN(num5))
				{
					base.Add(this.input.GetDateTime(index), num5);
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
				double result = 0.0;
				if (index > 1)
				{
					if (num > num2)
					{
						result = OBV.Value(input, index - 1) + num3;
					}
					if (num < num2)
					{
						result = OBV.Value(input, index - 1) - num3;
					}
					if (num == num2)
					{
						result = OBV.Value(input, index - 1);
					}
				}
				else
				{
					if (num > num2)
					{
						result = num3;
					}
					if (num < num2)
					{
						result = -num3;
					}
					if (num == num2)
					{
						result = 0.0;
					}
				}
				return result;
			}
			return double.NaN;
		}
	}
}
