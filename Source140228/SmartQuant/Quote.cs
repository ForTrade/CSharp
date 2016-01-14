using System;
namespace SmartQuant
{
	public class Quote : DataObject
	{
		internal Bid bid;
		internal Ask ask;
		public override byte TypeId
		{
			get
			{
				return 5;
			}
		}
		public Bid Bid
		{
			get
			{
				return this.bid;
			}
		}
		public Ask Ask
		{
			get
			{
				return this.ask;
			}
		}
		public Quote(Bid bid, Ask ask)
		{
			this.bid = bid;
			this.ask = ask;
			if (bid.dateTime > ask.DateTime)
			{
				this.dateTime = bid.dateTime;
				return;
			}
			this.dateTime = ask.dateTime;
		}
		public Quote(DateTime dateTime, byte providerId, int instrumentId, double bidPrice, int bidSize, double askPrice, int askSize) : this(new Bid(dateTime, providerId, instrumentId, bidPrice, bidSize), new Ask(dateTime, providerId, instrumentId, askPrice, askSize))
		{
		}
	}
}
