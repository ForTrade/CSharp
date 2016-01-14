using System;
using System.Collections.Generic;
using System.Threading;
namespace SmartQuant
{
	public class DataSimulator : Provider, IDataSimulator, IDataProvider, IProvider
	{
		private Thread thread;
		private LinkedList<DataSeriesObject> seriesObjects = new LinkedList<DataSeriesObject>();
		private long count;
		private DateTime dateTime1;
		private DateTime dateTime2;
		private bool isExiting;
		private bool isRunning;
		private List<DataSeries> series = new List<DataSeries>();
		public DateTime DateTime1
		{
			get
			{
				return this.dateTime1;
			}
			set
			{
				this.dateTime1 = value;
			}
		}
		public DateTime DateTime2
		{
			get
			{
				return this.dateTime2;
			}
			set
			{
				this.dateTime2 = value;
			}
		}
		public List<DataSeries> Series
		{
			get
			{
				return this.series;
			}
			set
			{
				this.series = value;
			}
		}
		public DataSimulator(Framework framework) : base(framework)
		{
			this.id = 1;
			this.name = "DataSimulator";
			this.description = "Default data simulator";
			this.url = "www.smartquant.com";
			this.dateTime1 = DateTime.MinValue;
			this.dateTime2 = DateTime.MaxValue;
		}
		public override void Connect()
		{
			if (!base.IsConnected)
			{
				base.Status = ProviderStatus.Connected;
			}
		}
		public override void Disconnect()
		{
			if (!base.IsDisconnected)
			{
				this.isExiting = true;
				while (this.isRunning)
				{
					Thread.Sleep(1);
				}
				this.Clear();
				base.Status = ProviderStatus.Disconnected;
			}
		}
		protected override void OnConnected()
		{
			if (this.series.Count != 0)
			{
				foreach (DataSeries current in this.series)
				{
					EventQueue eventQueue = new EventQueue(1, 0, 2, 25000);
					eventQueue.name = current.name;
					eventQueue.Enqueue(new OnQueueOpened());
					this.framework.eventBus.dataPipe.Add(eventQueue);
					if (this.seriesObjects.Count == 0)
					{
						eventQueue.Enqueue(new OnSimulatorStart());
					}
					this.seriesObjects.Add(new DataSeriesObject(current, this.dateTime1, this.dateTime2, eventQueue));
				}
			}
		}
		protected override void OnDisconnected()
		{
		}
		private void Subscribe_(Instrument instrument, DateTime dateTime1, DateTime dateTime2)
		{
			if (this.series.Count != 0)
			{
				return;
			}
			Console.WriteLine(DateTime.Now + " DataSimulator::Subscribe " + instrument.symbol);
			DataSeries dataSeries = this.framework.DataManager.GetSeries(instrument, 4);
			if (dataSeries != null)
			{
				EventQueue eventQueue = new EventQueue(1, 0, 2, 25000);
				eventQueue.name = instrument + " trade";
				eventQueue.Enqueue(new OnQueueOpened());
				this.framework.eventBus.dataPipe.Add(eventQueue);
				if (this.seriesObjects.Count == 0)
				{
					eventQueue.Enqueue(new OnSimulatorStart());
				}
				this.seriesObjects.Add(new DataSeriesObject(dataSeries, dateTime1, dateTime2, eventQueue));
			}
			dataSeries = this.framework.DataManager.GetSeries(instrument, 2);
			if (dataSeries != null)
			{
				EventQueue eventQueue = new EventQueue(1, 0, 2, 25000);
				eventQueue.Enqueue(new OnQueueOpened());
				eventQueue.name = instrument + " bid";
				this.framework.eventBus.dataPipe.Add(eventQueue);
				this.seriesObjects.Add(new DataSeriesObject(dataSeries, dateTime1, dateTime2, eventQueue));
			}
			dataSeries = this.framework.DataManager.GetSeries(instrument, 3);
			if (dataSeries != null)
			{
				EventQueue eventQueue = new EventQueue(1, 0, 2, 25000);
				eventQueue.Enqueue(new OnQueueOpened());
				eventQueue.name = instrument + " ask";
				this.framework.eventBus.dataPipe.Add(eventQueue);
				this.seriesObjects.Add(new DataSeriesObject(dataSeries, dateTime1, dateTime2, eventQueue));
			}
			dataSeries = this.framework.DataManager.GetSeries(instrument, 5);
			if (dataSeries != null)
			{
				EventQueue eventQueue = new EventQueue(1, 0, 2, 25000);
				eventQueue.Enqueue(new OnQueueOpened());
				eventQueue.name = instrument + " quote";
				this.framework.eventBus.dataPipe.Add(eventQueue);
				this.seriesObjects.Add(new DataSeriesObject(dataSeries, dateTime1, dateTime2, eventQueue));
			}
			dataSeries = this.framework.DataManager.GetSeries(instrument, 6);
			if (dataSeries != null)
			{
				EventQueue eventQueue = new EventQueue(1, 0, 2, 25000);
				eventQueue.Enqueue(new OnQueueOpened());
				eventQueue.name = instrument + " bar";
				this.framework.eventBus.dataPipe.Add(eventQueue);
				this.seriesObjects.Add(new DataSeriesObject(dataSeries, dateTime1, dateTime2, eventQueue));
			}
			dataSeries = this.framework.DataManager.GetSeries(instrument, 7);
			if (dataSeries != null)
			{
				EventQueue eventQueue = new EventQueue(1, 0, 2, 25000);
				eventQueue.Enqueue(new OnQueueOpened());
				eventQueue.name = instrument + " level2";
				this.framework.eventBus.dataPipe.Add(eventQueue);
				this.seriesObjects.Add(new DataSeriesObject(dataSeries, dateTime1, dateTime2, eventQueue));
			}
			dataSeries = this.framework.DataManager.GetSeries(instrument, 22);
			if (dataSeries != null)
			{
				EventQueue eventQueue = new EventQueue(1, 0, 2, 25000);
				eventQueue.Enqueue(new OnQueueOpened());
				eventQueue.name = instrument + " fundamental";
				this.framework.eventBus.dataPipe.Add(eventQueue);
				this.seriesObjects.Add(new DataSeriesObject(dataSeries, dateTime1, dateTime2, eventQueue));
			}
			dataSeries = this.framework.DataManager.GetSeries(instrument, 23);
			if (dataSeries != null)
			{
				EventQueue eventQueue = new EventQueue(1, 0, 2, 25000);
				eventQueue.Enqueue(new OnQueueOpened());
				eventQueue.name = instrument + " news";
				this.framework.eventBus.dataPipe.Add(eventQueue);
				this.seriesObjects.Add(new DataSeriesObject(dataSeries, dateTime1, dateTime2, eventQueue));
			}
		}
		public override void Subscribe(Instrument instrument)
		{
			if (!this.isRunning)
			{
				this.Subscribe_(instrument, this.dateTime1, this.dateTime2);
				this.Run();
				return;
			}
			this.Subscribe_(instrument, this.framework.Clock.DateTime, this.dateTime2);
		}
		public override void Subscribe(InstrumentList instruments)
		{
			if (!this.isRunning)
			{
				foreach (Instrument current in instruments)
				{
					this.Subscribe_(current, this.dateTime1, this.dateTime2);
				}
				this.Run();
				return;
			}
			foreach (Instrument current2 in instruments)
			{
				this.Subscribe_(current2, this.framework.Clock.DateTime, this.dateTime2);
			}
		}
		private void Run()
		{
			this.thread = new Thread(new ThreadStart(this.ThreadRun));
			this.thread.Name = "Data Simulator Thread";
			this.thread.IsBackground = true;
			this.thread.Start();
		}
		private void ThreadRun()
		{
			Console.WriteLine(DateTime.Now + " Data simulator thread started");
			if (!base.IsConnected)
			{
				this.Connect();
			}
			this.isRunning = true;
			this.isExiting = false;
			while (!this.isExiting)
			{
				LinkedListNode<DataSeriesObject> linkedListNode = this.seriesObjects.First;
				LinkedListNode<DataSeriesObject> linkedListNode2 = null;
				while (linkedListNode != null)
				{
					DataSeriesObject data = linkedListNode.Data;
					if (data.obj != null)
					{
						if (data.Enqueue())
						{
							this.count += 1L;
						}
						linkedListNode2 = linkedListNode;
					}
					else
					{
						if (linkedListNode2 == null)
						{
							this.seriesObjects.First = linkedListNode.Next;
						}
						else
						{
							linkedListNode2.Next = linkedListNode.Next;
						}
						this.seriesObjects.Count--;
						data.queue.Enqueue(new OnQueueClosed());
					}
					linkedListNode = linkedListNode.Next;
				}
			}
			this.isExiting = false;
			this.isRunning = false;
			Console.WriteLine(DateTime.Now + " Data simulator thread stopped");
		}
		public void Clear()
		{
			this.seriesObjects.Clear();
			this.dateTime1 = DateTime.MinValue;
			this.dateTime2 = DateTime.MaxValue;
		}
	}
}
