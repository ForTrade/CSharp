using System;
namespace SmartQuant
{
	public class PositionEventArgs : PortfolioEventArgs
	{
		public Position Position
		{
			get;
			private set;
		}
		public PositionEventArgs(Portfolio portfolio, Position position) : base(portfolio)
		{
			this.Position = position;
		}
	}
}
