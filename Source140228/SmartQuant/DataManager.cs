using System;
using System.Collections.Generic;
using System.Threading;
namespace SmartQuant
{
	public class DataManager
	{
		private Framework framework;
		private DataServer server;
		private IdArray<Bid> bid = new IdArray<Bid>(1000);
		private IdArray<Ask> ask = new IdArray<Ask>(1000);
		private IdArray<Trade> trade = new IdArray<Trade>(1000);
		private IdArray<Bar> bar = new IdArray<Bar>(1000);
		private IdArray<OrderBook> book = new IdArray<OrderBook>(1000);
		private IdArray<News> news = new IdArray<News>(1000);
		private IdArray<Fundamental> fundamental = new IdArray<Fundamental>(1000);
		private ManualResetEventSlim handle;
		private List<HistoricalData> historicalData;
		public DataServer Server
		{
			get
			{
				return this.server;
			}
		}
		public DataManager(Framework framework, DataServer server)
		{
			this.framework = framework;
			this.server = server;
			if (server != null)
			{
				server.Open();
			}
		}
		public DataSeries GetSeries(Instrument instrument, byte type)
		{
			return this.server.GetDataSeries(instrument, type);
		}
		public DataSeries GetSeries(string name)
		{
			return this.server.GetDataSeries(name);
		}
		public void DeleteSeries(Instrument instrument, byte type)
		{
			this.server.DeleteDataSeries(instrument, type);
		}
		public void DeleteSeries(string name)
		{
			this.server.DeleteDataSeries(name);
		}
		public void Save(Instrument instrument, DataObject obj)
		{
			this.server.Save(instrument, obj);
		}
		public void Save(int instrumentId, DataObject obj)
		{
			Instrument byId = this.framework.InstrumentManager.GetById(instrumentId);
			this.server.Save(byId, obj);
		}
		public Bid GetBid(Instrument instrument)
		{
			return this.bid[instrument.id];
		}
		public Ask GetAsk(Instrument instrument)
		{
			return this.ask[instrument.id];
		}
		public Trade GetTrade(Instrument instrument)
		{
			return this.trade[instrument.id];
		}
		public Bar GetBar(Instrument instrument)
		{
			return this.bar[instrument.id];
		}
		public OrderBook GetOrderBook(Instrument instrument)
		{
			return this.book[instrument.id];
		}
		public TickSeries GetHistoricalTrades(IHistoricalDataProvider provider, Instrument instrument, DateTime dateTime1, DateTime dateTime2)
		{
			HistoricalDataRequest request = new HistoricalDataRequest(instrument, dateTime1, dateTime2, 4);
			provider.Send(request);
			this.handle = new ManualResetEventSlim(false);
			this.handle.Wait();
			TickSeries tickSeries = new TickSeries("");
			if (this.historicalData != null)
			{
				foreach (HistoricalData current in this.historicalData)
				{
					DataObject[] objects = current.Objects;
					for (int i = 0; i < objects.Length; i++)
					{
						DataObject dataObject = objects[i];
						tickSeries.Add((Trade)dataObject);
					}
				}
			}
			this.historicalData = null;
			return tickSeries;
		}
		internal void OnBid(Bid bid)
		{
			this.bid[bid.instrumentId] = bid;
		}
		internal void OnAsk(Ask ask)
		{
			this.ask[ask.instrumentId] = ask;
		}
		internal void OnTrade(Trade trade)
		{
			this.trade[trade.instrumentId] = trade;
		}
		internal void OnBar(Bar bar)
		{
			this.bar[bar.instrumentId] = bar;
		}
		internal void OnLevel2Snapshot(Level2Snapshot snapshot)
		{
			OrderBook orderBook = this.book[snapshot.instrumentId];
			if (orderBook == null)
			{
				orderBook = new OrderBook();
				this.book[snapshot.instrumentId] = orderBook;
			}
			orderBook.Set(snapshot);
		}
		internal void OnLevel2Update(Level2Update update)
		{
			OrderBook orderBook = this.book[update.instrumentId];
			if (orderBook == null)
			{
				orderBook = new OrderBook();
				this.book[update.instrumentId] = orderBook;
			}
			orderBook.Set(update);
		}
		internal void OnNews(News news)
		{
			this.news[news.instrumentId] = news;
		}
		internal void OnFundamental(Fundamental fundamental)
		{
			this.fundamental[fundamental.instrumentId] = fundamental;
		}
		internal void OnHistoricalData(HistoricalData data)
		{
			if (this.historicalData == null)
			{
				this.historicalData = new List<HistoricalData>();
			}
			this.historicalData.Add(data);
		}
		internal void OnHistoricalDataEnd(HistoricalDataEnd end)
		{
			this.handle.Set();
		}
		public void Clear()
		{
			this.bid.Clear();
			this.ask.Clear();
			this.trade.Clear();
			this.bar.Clear();
			this.book.Clear();
		}
	}
}
