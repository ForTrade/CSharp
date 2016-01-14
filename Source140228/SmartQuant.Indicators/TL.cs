using System;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class TL : Indicator
	{
		public TL(ISeries input) : base(input)
		{
			this.Init();
		}
		protected override void Init()
		{
			this.name = "TL";
			this.description = "True Low";
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
			double num = TL.Value(this.input, index);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index)
		{
			if (index >= 1)
			{
				double val = input[index, BarData.Low];
				double val2 = input[index - 1, BarData.Close];
				return Math.Min(val, val2);
			}
			return double.NaN;
		}
	}
}
