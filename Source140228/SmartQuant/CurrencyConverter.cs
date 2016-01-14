using System;
namespace SmartQuant
{
	public class CurrencyConverter : ICurrencyConverter
	{
		private Framework framework;
		public CurrencyConverter(Framework framework)
		{
			this.framework = framework;
		}
		public virtual double Convert(double amount, byte fromCurrencyId, byte toCurrencyId)
		{
			return amount;
		}
	}
}
