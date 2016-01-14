using System;
namespace SmartQuant
{
	public class Bid : Tick
	{
		public override byte TypeId
		{
			get
			{
				return 2;
			}
		}
		public Bid(DateTime dateTime, byte providerId, int instrumentId, double price, int size) : base(dateTime, providerId, instrumentId, price, size)
		{
		}
		public Bid(DateTime dateTime, DateTime exchangeDateTime, byte providerId, int instrumentId, double price, int size) : base(dateTime, exchangeDateTime, providerId, instrumentId, price, size)
		{
		}
		public Bid()
		{
		}
		public Bid(Bid bid) : base(bid)
		{
		}
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Bid ",
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
