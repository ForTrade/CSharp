using System;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class MDM : Indicator
	{
		public MDM(ISeries input) : base(input)
		{
			this.Init();
		}
		protected override void Init()
		{
			this.name = "MDM";
			this.description = "Minus Directional Movement";
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
			double num = MDM.Value(this.input, index);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index)
		{
			if (index < 1)
			{
				return double.NaN;
			}
			double num = input[index, BarData.High];
			double num2 = input[index, BarData.Low];
			double num3 = input[index - 1, BarData.High];
			double num4 = input[index - 1, BarData.Low];
			double num5 = 0.0;
			double num6 = 0.0;
			if (num > num3)
			{
				num5 = num - num3;
			}
			if (num2 < num4)
			{
				num6 = num4 - num2;
			}
			if (num6 > num5)
			{
				return num6;
			}
			return 0.0;
		}
	}
}
