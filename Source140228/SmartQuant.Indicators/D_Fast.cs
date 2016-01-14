using System;
using System.ComponentModel;
namespace SmartQuant.Indicators
{
	[Serializable]
	public class D_Fast : Indicator
	{
		protected int length;
		protected int order;
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
		public int Order
		{
			get
			{
				return this.order;
			}
			set
			{
				this.order = value;
				this.Init();
			}
		}
		public D_Fast(ISeries input, int length, int order) : base(input)
		{
			this.length = length;
			this.order = order;
			this.Init();
		}
		protected override void Init()
		{
			this.name = string.Concat(new object[]
			{
				"%D Fast (",
				this.length,
				", ",
				this.order,
				")"
			});
			this.description = "%D Fast";
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
			double num = D_Fast.Value(this.input, index, this.length, this.order);
			if (!double.IsNaN(num))
			{
				base.Add(this.input.GetDateTime(index), num);
			}
		}
		public static double Value(ISeries input, int index, int length, int order)
		{
			if (index >= length + order - 1)
			{
				double num = 0.0;
				for (int i = index; i > index - order; i--)
				{
					num += K_Fast.Value(input, i, length);
				}
				return num / (double)order;
			}
			return double.NaN;
		}
	}
}
