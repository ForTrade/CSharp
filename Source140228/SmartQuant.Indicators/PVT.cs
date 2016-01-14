using System;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class PVT : Indicator
	{
		public PVT(ISeries input) : base(input)
		{
			this.Init();
		}
		protected override void Init()
		{
			this.name = "PVT";
			this.description = "Price and Volume Trend";
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
				double num5;
				if (index >= 2)
				{
					int num4 = -1;
					num5 = (num - num2) / num2 * num3 + this[index - 1 + num4];
				}
				else
				{
					num5 = (num - num2) / num2 * num3;
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
				double result;
				if (index >= 2)
				{
					result = (num - num2) / num2 * num3 + PVT.Value(input, index - 1);
				}
				else
				{
					result = (num - num2) / num2 * num3;
				}
				return result;
			}
			return double.NaN;
		}
	}
}
