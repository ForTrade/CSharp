using System;
namespace SmartQuant
{
	public interface ISlippageProvider
	{
		double Slippage
		{
			get;
			set;
		}
		double GetPrice(ExecutionReport report);
	}
}
