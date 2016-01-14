using System;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class MarketFI : Indicator
	{
		public MarketFI(ISeries input) : base(input)
		{
			this.Init();
		}
		protected override void Init()
		{
			this.name = "MarketFI";
			this.description = "Market Force Index";
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
			double num = MarketFI.Value(this.input, index);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index)
		{
			if (index >= 0)
			{
				double num = input[index, BarData.High];
				double num2 = input[index, BarData.Low];
				double num3 = input[index, BarData.Volume];
				double result;
				if (num3 != 0.0)
				{
					result = (num - num2) / num3 * 1000.0;
				}
				else
				{
					result = 0.0;
				}
				return result;
			}
			return double.NaN;
		}
	}
}
