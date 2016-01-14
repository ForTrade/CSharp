using System;
namespace SmartQuant
{
	public class OnPositionClosed : Event
	{
		public Portfolio portfolio;
		public Position position;
		public override byte TypeId
		{
			get
			{
				return 111;
			}
		}
		public OnPositionClosed(Portfolio portfolio, Position position)
		{
			this.portfolio = portfolio;
			this.position = position;
		}
		public override string ToString()
		{
			return "OnPositionClosed " + this.position;
		}
	}
}
