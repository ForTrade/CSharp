using System;
namespace SmartQuant
{
	public class OnFill : Event
	{
		internal Fill fill;
		internal Portfolio portfolio;
		public Fill Fill
		{
			get
			{
				return this.fill;
			}
		}
		public Portfolio Portfolio
		{
			get
			{
				return this.portfolio;
			}
		}
		public override byte TypeId
		{
			get
			{
				return 113;
			}
		}
		public OnFill(Portfolio portfolio, Fill fill)
		{
			this.portfolio = portfolio;
			this.fill = fill;
		}
		public override string ToString()
		{
			return "OnFill " + this.fill.ToString();
		}
	}
}
