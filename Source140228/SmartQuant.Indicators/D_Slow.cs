using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class D_Slow : Indicator
	{
		protected int length;
		protected int order1;
		protected int order2;
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
		public int Order1
		{
			get
			{
				return this.order1;
			}
			set
			{
				this.order1 = value;
				this.Init();
			}
		}
		[Category("Parameters"), Description("")]
		public int Order2
		{
			get
			{
				return this.order2;
			}
			set
			{
				this.order2 = value;
				this.Init();
			}
		}
		public D_Slow(ISeries input, int length, int order1, int order2) : base(input)
		{
			this.length = length;
			this.order1 = order1;
			this.order2 = order2;
			this.Init();
		}
		protected override void Init()
		{
			this.name = string.Concat(new object[]
			{
				"%D Slow (",
				this.length,
				", ",
				this.order1,
				", ",
				this.order2,
				")"
			});
			this.description = "%D Slow";
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
			double num = D_Slow.Value(this.input, index, this.length, this.order1, this.order2);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int length, int order1, int order2)
		{
			if (index >= length + order1 + order2 - 1)
			{
				double num = 0.0;
				for (int i = index; i > index - order2; i--)
				{
					num += K_Slow.Value(input, i, length, order1);
				}
				return num / (double)order2;
			}
			return double.NaN;
		}
	}
}
