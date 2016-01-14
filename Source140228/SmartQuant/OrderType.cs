using System;
namespace SmartQuant
{
	public enum OrderType : byte
	{
		Market,
		Stop,
		Limit,
		StopLimit,
		MarketOnClose,
		TrailingStop,
		TrailingStopLimit
	}
}
