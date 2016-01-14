using System;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class TR : Indicator
	{
		public TR(ISeries input) : base(input)
		{
			this.Init();
		}
		protected override void Init()
		{
			this.name = "TR";
			this.description = "True Range";
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
			double num = TR.Value(this.input, index);
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
				double num2 = input[index, BarData.Low];
				double num3 = input[index - 1, BarData.Close];
				double val = Math.Abs(num - num2);
				double val2 = Math.Abs(num - num3);
				double val3 = Math.Abs(num3 - num2);
				return Math.Max(val, Math.Max(val2, val3));
			}
			return double.NaN;
		}
	}
}
