using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class VROC : Indicator
	{
		protected int length;
		[Category("Parameters"), Description("")]
		public int Length
		{
			get
			{
				return this.length;
			}
			set
			{
				this.length = value;
				this.Init();
			}
		}
		public VROC(ISeries input, int length) : base(input)
		{
			this.length = length;
			this.Init();
		}
		protected override void Init()
		{
			this.name = "VROC (" + this.length + ")";
			this.description = "Volume Rate of Change";
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
			double num = VROC.Value(this.input, index, this.length);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int length)
		{
			if (index >= length - 1)
			{
				return (input[index, BarData.Volume] - input[index - length + 1, BarData.Volume]) / input[index - length + 1, BarData.Volume] * 100.0;
			}
			return double.NaN;
		}
	}
}
