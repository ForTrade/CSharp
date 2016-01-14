using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class BBL : Indicator
	{
		protected int length;
		protected double k;
		protected BarData barData;
		[Category("Parameters"), Description("")]
		public BarData BarData
		{
			get
			{
				return this.barData;
			}
			set
			{
				this.barData = value;
				this.Init();
			}
		}
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
		[Category("Parameters"), Description("")]
		public double K
		{
			get
			{
				return this.k;
			}
			set
			{
				this.k = value;
				this.Init();
			}
		}
		public BBL(ISeries input, int length, double k, BarData barData = BarData.Close) : base(input)
		{
			this.length = length;
			this.barData = barData;
			this.k = k;
			this.Init();
		}
		protected override void Init()
		{
			this.name = string.Concat(new object[]
			{
				"BBL (",
				this.length,
				" ,",
				this.k,
				", ",
				this.barData,
				")"
			});
			this.description = "Bollinger Band Lower";
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
			double num = BBL.Value(this.input, index, this.length, this.k, this.barData);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int length, double k, BarData barData = BarData.Close)
		{
			if (index >= length - 1)
			{
				return SMA.Value(input, index, length, barData) - k * SMD.Value(input, index, length, barData);
			}
			return double.NaN;
		}
	}
}
