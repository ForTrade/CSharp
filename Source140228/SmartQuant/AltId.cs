using System;
namespace SmartQuant
{
	public class AltId
	{
		internal byte providerId;
		internal string symbol;
		internal string exchange;
		public byte ProviderId
		{
			get
			{
				return this.providerId;
			}
			set
			{
				this.providerId = value;
			}
		}
		public string Symbol
		{
			get
			{
				return this.symbol;
			}
			set
			{
				this.symbol = value;
			}
		}
		public string Exchange
		{
			get
			{
				return this.exchange;
			}
			set
			{
				this.exchange = value;
			}
		}
		public AltId()
		{
		}
		public AltId(byte providerId, string symbol, string exchange)
		{
			this.providerId = providerId;
			this.symbol = symbol;
			this.exchange = exchange;
		}
		public override string ToString()
		{
			return string.Format("[{0}] {1}@{2}", this.providerId, this.symbol, this.exchange);
		}
	}
}
