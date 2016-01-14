using System;
namespace SmartQuant
{
	public class Pricer
	{
		private Framework framework;
		public Pricer(Framework framework)
		{
			this.framework = framework;
		}
		public virtual double GetPrice(Position position)
		{
			Trade trade = this.framework.DataManager.GetTrade(position.instrument);
			if (trade != null)
			{
				return trade.price;
			}
			return 0.0;
		}
	}
}
