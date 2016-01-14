using System;
namespace SmartQuant
{
	public class OnPositionOpened : Event
	{
		public Portfolio portfolio;
		public Position position;
		public override byte TypeId
		{
			get
			{
				return 110;
			}
		}
		public OnPositionOpened(Portfolio portfolio, Position position)
		{
			this.portfolio = portfolio;
			this.position = position;
		}
		public override string ToString()
		{
			return "OnPositionOpened " + this.position;
		}
	}
}
