using System;
namespace SmartQuant
{
	public class OnPositionChanged : Event
	{
		public Portfolio portfolio;
		public Position position;
		public override byte TypeId
		{
			get
			{
				return 112;
			}
		}
		public OnPositionChanged(Portfolio portfolio, Position position)
		{
			this.portfolio = portfolio;
			this.position = position;
		}
		public override string ToString()
		{
			return "OnPositionChanged " + this.position;
		}
	}
}
