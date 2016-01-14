using System;
using System.Collections.Generic;
namespace SmartQuant
{
	public class OrderBook
	{
		internal IList<Tick> bids;
		internal IList<Tick> asks;
		public IList<Tick> Bids
		{
			get
			{
				return this.bids;
			}
		}
		public IList<Tick> Asks
		{
			get
			{
				return this.asks;
			}
		}
		public OrderBook()
		{
			this.bids = new List<Tick>();
			this.asks = new List<Tick>();
		}
		public Quote GetQuote(int level)
		{
			Bid bid = new Bid();
			Ask ask = new Ask();
			if (this.bids.Count < level)
			{
				Tick tick = this.bids[level];
				bid = new Bid(tick.dateTime, tick.providerId, tick.instrumentId, tick.price, tick.size);
			}
			if (this.asks.Count < level)
			{
				Tick tick2 = this.asks[level];
				ask = new Ask(tick2.dateTime, tick2.providerId, tick2.instrumentId, tick2.price, tick2.size);
			}
			return new Quote(bid, ask);
		}
		public int GetBidVolume()
		{
			return this.GetVolume(this.bids);
		}
		public int GetAskVolume()
		{
			return this.GetVolume(this.asks);
		}
		public double GetAvgBidPrice()
		{
			return this.GetAvgPrice(this.bids);
		}
		public double GetAvgAskPrice()
		{
			return this.GetAvgPrice(this.asks);
		}
		internal void Set(Level2Snapshot snapshot)
		{
			this.bids.Clear();
			this.asks.Clear();
			Bid[] array = snapshot.bids;
			for (int i = 0; i < array.Length; i++)
			{
				Bid tick = array[i];
				this.bids.Add(new Tick(tick));
			}
			Ask[] array2 = snapshot.asks;
			for (int j = 0; j < array2.Length; j++)
			{
				Ask tick2 = array2[j];
				this.asks.Add(new Tick(tick2));
			}
		}
		internal void Set(Level2Update update)
		{
			Level2[] entries = update.entries;
			for (int i = 0; i < entries.Length; i++)
			{
				Level2 level = entries[i];
				IList<Tick> list = null;
				switch (level.side)
				{
				case Level2Side.Bid:
					list = this.bids;
					break;
				case Level2Side.Ask:
					list = this.asks;
					break;
				}
				switch (level.action)
				{
				case Level2UpdateAction.New:
					list.Insert(level.position, new Tick(level));
					break;
				case Level2UpdateAction.Change:
					list[level.position].size = level.size;
					break;
				case Level2UpdateAction.Delete:
					list.RemoveAt(level.position);
					break;
				case Level2UpdateAction.Reset:
					list.Clear();
					break;
				}
			}
		}
		private int GetVolume(IList<Tick> ticks)
		{
			int num = 0;
			foreach (Tick current in ticks)
			{
				num += current.size;
			}
			return num;
		}
		private double GetAvgPrice(IList<Tick> ticks)
		{
			double num = 0.0;
			double num2 = 0.0;
			foreach (Tick current in ticks)
			{
				num += current.price * (double)current.size;
				num2 += (double)current.size;
			}
			return num / num2;
		}
	}
}
