using System;
namespace SmartQuant
{
	public class PortfolioEventArgs : EventArgs
	{
		public Portfolio Portfolio
		{
			get;
			private set;
		}
		public PortfolioEventArgs(Portfolio portfolio)
		{
			this.Portfolio = portfolio;
		}
	}
}
