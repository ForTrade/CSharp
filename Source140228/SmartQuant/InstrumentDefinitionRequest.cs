using System;
namespace SmartQuant
{
	public class InstrumentDefinitionRequest
	{
		public string Id
		{
			get;
			set;
		}
		public InstrumentType? FilterType
		{
			get;
			set;
		}
		public string FilterSymbol
		{
			get;
			set;
		}
		public string FilterExchange
		{
			get;
			set;
		}
	}
}
