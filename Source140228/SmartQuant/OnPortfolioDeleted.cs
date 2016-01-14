using System;
namespace SmartQuant
{
	public class OnPortfolioDeleted : Event
	{
		internal Portfolio portfolio;
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
				return 128;
			}
		}
		public OnPortfolioDeleted(Portfolio portfolio)
		{
			this.portfolio = portfolio;
		}
	}
}
