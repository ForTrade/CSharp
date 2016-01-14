using System;
namespace SmartQuant
{
	public class Ask : Tick
	{
		public override byte TypeId
		{
			get
			{
				return 3;
			}
		}
		public Ask(DateTime dateTime, byte providerId, int instrument, double price, int size) : base(dateTime, providerId, instrument, price, size)
		{
		}
		public Ask(DateTime dateTime, DateTime exchangeDateTime, byte providerId, int instrument, double price, int size) : base(dateTime, exchangeDateTime, providerId, instrument, price, size)
		{
		}
		public Ask()
		{
		}
		public Ask(Ask ask) : base(ask)
		{
		}
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Ask ",
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
