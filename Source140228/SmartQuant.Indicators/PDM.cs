using System;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class PDM : Indicator
	{
		public PDM(ISeries input) : base(input)
		{
			this.Init();
		}
		protected override void Init()
		{
			this.name = "PDM";
			this.description = "Plus Directional Movement";
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
			double num = PDM.Value(this.input, index);
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
			if (num5 > num6)
			{
				return num5;
			}
			return 0.0;
		}
	}
}
