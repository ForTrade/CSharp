using System;
namespace SmartQuant
{
	public class Tick : DataObject
	{
		protected internal DateTime exchangeDateTime;
		protected internal byte providerId;
		protected internal int instrumentId;
		protected internal double price;
		protected internal int size;
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
		public int InstrumentId
		{
			get
			{
				return this.instrumentId;
			}
			set
			{
				this.instrumentId = value;
			}
		}
		public double Price
		{
			get
			{
				return this.price;
			}
			set
			{
				this.price = value;
			}
		}
		public int Size
		{
			get
			{
				return this.size;
			}
			set
			{
				this.size = value;
			}
		}
		public DateTime ExchangeDateTime
		{
			get
			{
				return this.exchangeDateTime;
			}
			set
			{
				this.exchangeDateTime = value;
			}
		}
		public Tick()
		{
		}
		public Tick(Tick tick) : base(tick)
		{
			this.providerId = tick.providerId;
			this.instrumentId = tick.instrumentId;
			this.price = tick.price;
			this.size = tick.size;
		}
		public Tick(DateTime dateTime, byte providerId, int instrumentId, double price, int size) : base(dateTime)
		{
			this.providerId = providerId;
			this.instrumentId = instrumentId;
			this.price = price;
			this.size = size;
		}
		public Tick(DateTime dateTime, DateTime exchangeDateTime, byte providerId, int instrumentId, double price, int size) : base(dateTime)
		{
			this.exchangeDateTime = exchangeDateTime;
			this.providerId = providerId;
			this.instrumentId = instrumentId;
			this.price = price;
			this.size = size;
		}
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Tick ",
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
