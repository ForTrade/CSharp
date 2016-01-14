using System;
namespace SmartQuant
{
	public interface ICurrencyConverter
	{
		double Convert(double amount, byte fromCurrencyId, byte toCurrencyId);
	}
}
