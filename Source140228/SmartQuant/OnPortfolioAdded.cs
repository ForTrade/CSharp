using System;
namespace SmartQuant
{
	public class OnPortfolioAdded : Event
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
				return 127;
			}
		}
		public OnPortfolioAdded(Portfolio portfolio)
		{
			this.portfolio = portfolio;
		}
	}
}
