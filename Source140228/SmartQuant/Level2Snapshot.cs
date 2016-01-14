using System;
namespace SmartQuant
{
	public class Level2Snapshot : DataObject
	{
		internal byte providerId;
		internal int instrumentId;
		internal Bid[] bids;
		internal Ask[] asks;
		public override byte TypeId
		{
			get
			{
				return 8;
			}
		}
		public Bid[] Bids
		{
			get
			{
				return this.bids;
			}
		}
		public Ask[] Asks
		{
			get
			{
				return this.asks;
			}
		}
		public Level2Snapshot(DateTime dateTime, byte providerId, int instrumentId, Bid[] bids, Ask[] asks) : base(dateTime)
		{
			this.providerId = providerId;
			this.instrumentId = instrumentId;
			this.bids = bids;
			this.asks = asks;
		}
		public Level2Snapshot()
		{
		}
	}
}
