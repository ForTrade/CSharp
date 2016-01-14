using System;
namespace SmartQuant
{
	public class Trade : Tick
	{
		public override byte TypeId
		{
			get
			{
				return 4;
			}
		}
		public Trade(DateTime dateTime, byte providerId, int instrumentId, double price, int size) : base(dateTime, providerId, instrumentId, price, size)
		{
		}
		public Trade(DateTime dateTime, DateTime exchangeDateTime, byte providerId, int instrumentId, double price, int size) : base(dateTime, providerId, instrumentId, price, size)
		{
		}
		public Trade()
		{
		}
		public Trade(Trade trade) : base(trade)
		{
		}
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Trade ",
				this.dateTime,
				" ",
				this.providerId,
				" ",
				this.instrumentId,
				" ",
				this.price,
				" ",
				this.size
			});
		}
	}
}
