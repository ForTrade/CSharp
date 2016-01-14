using System;
namespace SmartQuant
{
	public enum InstrumentType : byte
	{
		Stock,
		Future,
		Option,
		FutureOption,
		Bond,
		FX,
		Index,
		ETF,
		MultiLeg,
		Synthetic
	}
}
