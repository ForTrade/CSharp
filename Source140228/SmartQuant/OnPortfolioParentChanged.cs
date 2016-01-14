using System;
namespace SmartQuant
{
	public class OnPortfolioParentChanged : Event
	{
		public Portfolio portfolio;
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
				return 129;
			}
		}
		public OnPortfolioParentChanged(Portfolio portfolio)
		{
			this.portfolio = portfolio;
		}
	}
}
