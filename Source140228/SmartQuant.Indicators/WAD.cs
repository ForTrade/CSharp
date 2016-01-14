using System;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class WAD : Indicator
	{
		public WAD(ISeries input) : base(input)
		{
			this.Init();
		}
		protected override void Init()
		{
			this.name = "WAD";
			this.description = "Williams Accumulation/Distribution";
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
			double value = 0.0;
			if (index >= 1)
			{
				double val = this.input[index, BarData.High];
				double val2 = this.input[index, BarData.Low];
				double num = this.input[index, BarData.Close];
				double num2 = this.input[index - 1, BarData.Close];
				double arg_68_0 = this.input[index, BarData.Volume];
				if (num > num2)
				{
					value = this[index - 1] + num - Math.Min(val2, num2);
				}
				if (num < num2)
				{
					value = this[index - 1] + num - Math.Max(val, num2);
				}
				if (num == num2)
				{
					value = this[index - 1];
				}
			}
			base.Add(this.input.GetDateTime(index), value);
		}
		public static double Value(ISeries input, int index)
		{
			double result = 0.0;
			if (index >= 1)
			{
				double val = input[index, BarData.High];
				double val2 = input[index, BarData.Low];
				double num = input[index, BarData.Close];
				double num2 = input[index - 1, BarData.Close];
				double arg_3D_0 = input[index, BarData.Volume];
				if (num > num2)
				{
					result = WAD.Value(input, index - 1) + num - Math.Min(val2, num2);
				}
				if (num < num2)
				{
					result = WAD.Value(input, index - 1) + num - Math.Max(val, num2);
				}
				if (num == num2)
				{
					result = WAD.Value(input, index - 1);
				}
			}
			return result;
		}
	}
}
