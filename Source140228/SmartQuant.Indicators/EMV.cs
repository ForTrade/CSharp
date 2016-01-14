using System;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class EMV : Indicator
	{
		public EMV(ISeries input) : base(input)
		{
			this.Init();
		}
		protected override void Init()
		{
			this.name = "EMV";
			this.description = "Ease of Movement";
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
			double num = EMV.Value(this.input, index);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index)
		{
			if (index >= 1)
			{
				double num = input[index, BarData.High];
				double num2 = input[index - 1, BarData.High];
				double num3 = input[index, BarData.Low];
				double num4 = input[index - 1, BarData.Low];
				double num5 = input[index, BarData.Volume];
				double num6 = (num + num3) / 2.0 - (num2 + num4) / 2.0;
				double num7 = num5 / 1000000.0 / (num - num3);
				return num6 / num7;
			}
			return double.NaN;
		}
	}
}
