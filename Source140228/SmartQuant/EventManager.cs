using System;
using System.Diagnostics;
using System.Threading;
namespace SmartQuant
{
	public class EventManager
	{
		private delegate void OnEventHandler(Event e);
		private Framework framework;
		internal EventBus bus;
		internal EventFilter filter;
		internal EventLogger logger;
		internal EventDispatcher dispatcher;
		internal BarFactory factory;
		internal EventManagerStatus status = EventManagerStatus.Stopped;
		private Thread thread;
		private bool threadExit;
		private Stopwatch watch = new Stopwatch();
		private long count;
		private long data_count;
		private bool step_enabled;
		private byte step_typeId = 6;
		private IdArray<EventManager.OnEventHandler> handler = new IdArray<EventManager.OnEventHandler>(1000);
		public EventManagerStatus Status
		{
			get
			{
				return this.status;
			}
		}
		public EventFilter Filter
		{
			get
			{
				return this.filter;
			}
			set
			{
				this.filter = value;
			}
		}
		public EventLogger Logger
		{
			get
			{
				return this.logger;
			}
			set
			{
				this.logger = value;
			}
		}
		public BarFactory BarFactory
		{
			get
			{
				return this.factory;
			}
			set
			{
				this.factory = value;
			}
		}
		public EventDispatcher Dispatcher
		{
			get
			{
				return this.dispatcher;
			}
			set
			{
				this.dispatcher = value;
			}
		}
		public long EventCount
		{
			get
			{
				return this.count;
			}
		}
		public long DataEventCount
		{
			get
			{
				return this.data_count;
			}
		}
		public EventManager(Framework framework, EventBus bus)
		{
			this.framework = framework;
			this.bus = bus;
			this.factory = new BarFactory(framework);
			this.dispatcher = new EventDispatcher(framework);
			this.handler[107] = new EventManager.OnEventHandler(this.OnSimulatorStart);
			this.handler[108] = new EventManager.OnEventHandler(this.OnSimulatorStop);
			this.handler[109] = new EventManager.OnEventHandler(this.OnSimulatorProgress);
			this.handler[2] = new EventManager.OnEventHandler(this.OnBid);
			this.handler[3] = new EventManager.OnEventHandler(this.OnAsk);
			this.handler[4] = new EventManager.OnEventHandler(this.OnTrade);
			this.handler[6] = new EventManager.OnEventHandler(this.OnBar);
			this.handler[8] = new EventManager.OnEventHandler(this.OnLevel2Snapshot);
			this.handler[9] = new EventManager.OnEventHandler(this.OnLevel2Update);
			this.handler[23] = new EventManager.OnEventHandler(this.OnNews);
			this.handler[22] = new EventManager.OnEventHandler(this.OnFundamental);
			this.handler[13] = new EventManager.OnEventHandler(this.OnExecutionReport);
			this.handler[116] = new EventManager.OnEventHandler(this.OnOrderStatusChanged);
			this.handler[117] = new EventManager.OnEventHandler(this.OnOrderPartiallyFilled);
			this.handler[118] = new EventManager.OnEventHandler(this.OnOrderFilled);
			this.handler[119] = new EventManager.OnEventHandler(this.OnOrderReplaced);
			this.handler[120] = new EventManager.OnEventHandler(this.OnOrderCancelled);
			this.handler[121] = new EventManager.OnEventHandler(this.OnOrderDone);
			this.handler[113] = new EventManager.OnEventHandler(this.OnFill);
			this.handler[110] = new EventManager.OnEventHandler(this.OnPositionOpened);
			this.handler[111] = new EventManager.OnEventHandler(this.OnPositionClosed);
			this.handler[112] = new EventManager.OnEventHandler(this.OnPositionChanged);
			this.handler[15] = new EventManager.OnEventHandler(this.OnReminder);
			this.handler[50] = new EventManager.OnEventHandler(this.OnGroup);
			this.handler[52] = new EventManager.OnEventHandler(this.OnGroupEvent);
			this.handler[130] = new EventManager.OnEventHandler(this.OnHistoricalData);
			this.handler[131] = new EventManager.OnEventHandler(this.OnHistoricalDataEnd);
			this.handler[140] = new EventManager.OnEventHandler(this.OnAccountData);
			if (bus != null)
			{
				this.thread = new Thread(new ThreadStart(this.ThreadRun));
				this.thread.Name = "Event Manager Thread";
				this.thread.IsBackground = true;
				this.thread.Start();
			}
		}
		public void Start()
		{
			if (this.status != EventManagerStatus.Running)
			{
				Console.WriteLine(DateTime.Now + " Event manager started at " + this.framework.clock.DateTime);
				this.status = EventManagerStatus.Running;
				this.OnEvent(new OnEventManagerStarted());
			}
		}
		public void Pause()
		{
			if (this.status != EventManagerStatus.Paused)
			{
				Console.WriteLine(DateTime.Now + " Event manager paused at " + this.framework.clock.DateTime);
				this.status = EventManagerStatus.Paused;
				this.OnEvent(new OnEventManagerPaused());
			}
		}
		public void Pause(DateTime dateTime)
		{
			this.framework.Clock.AddReminder(new ReminderCallback(this.OnPauseReminder), dateTime, null);
		}
		private void OnPauseReminder(DateTime dateTime, object data)
		{
			this.Pause();
		}
		public void Resume()
		{
			if (this.status != EventManagerStatus.Running)
			{
				Console.WriteLine(DateTime.Now + " Event manager resumed at " + this.framework.clock.DateTime);
				this.status = EventManagerStatus.Running;
				this.OnEvent(new OnEventManagerResumed());
			}
		}
		public void Stop()
		{
			if (this.status != EventManagerStatus.Stopped)
			{
				Console.WriteLine(DateTime.Now + " Event manager stopping at " + this.framework.clock.DateTime);
				this.status = EventManagerStatus.Stopping;
				if (this.framework.Mode == FrameworkMode.Simulation)
				{
					this.OnEvent(new OnSimulatorStop());
				}
				this.status = EventManagerStatus.Stopped;
				this.framework.eventBus.Clear();
				this.OnEvent(new OnEventManagerStopped());
				Console.WriteLine(DateTime.Now + " Event manager stopped at " + this.framework.clock.DateTime);
			}
		}
		public void Step(byte typeId = 0)
		{
			this.step_enabled = true;
			this.step_typeId = typeId;
			this.OnEvent(new OnEventManagerStep());
		}
		private void ThreadRun()
		{
			Console.WriteLine(string.Concat(new object[]
			{
				DateTime.Now,
				" Event manager thread started: Framework = ",
				this.framework.Name,
				" Clock = ",
				this.framework.Clock.GetModeAsString()
			}));
			this.status = EventManagerStatus.Running;
			while (!this.threadExit)
			{
				if (this.status == EventManagerStatus.Running || (this.status == EventManagerStatus.Paused && this.step_enabled))
				{
					Event e = this.bus.Dequeue();
					this.OnEvent(e);
				}
				else
				{
					Thread.Sleep(1);
				}
			}
		}
		public void OnEvent(Event e)
		{
			if (this.status == EventManagerStatus.Paused && this.step_enabled && (this.step_typeId == 0 || this.step_typeId == e.TypeId))
			{
				Console.WriteLine(DateTime.Now + " Event : " + e);
				this.step_enabled = false;
			}
			this.count += 1L;
			if (this.filter != null && this.filter.Filter(e) == null)
			{
				return;
			}
			if (this.handler[(int)e.TypeId] != null)
			{
				this.handler[(int)e.TypeId](e);
			}
			if (this.dispatcher != null)
			{
				this.dispatcher.OnEvent(e);
			}
			if (this.logger != null)
			{
				this.logger.OnEvent(e);
			}
		}
		private void OnSimulatorStart(Event e)
		{
			OnSimulatorStart arg_06_0 = (OnSimulatorStart)e;
			if (this.bus != null)
			{
				this.bus.ResetCounts();
			}
			this.count = 0L;
			this.data_count = 0L;
			this.watch.Reset();
			this.watch.Start();
		}
		private void OnSimulatorStop(Event e)
		{
			OnSimulatorStop arg_06_0 = (OnSimulatorStop)e;
			this.framework.strategyManager.Stop();
			this.watch.Stop();
			long elapsedMilliseconds = this.watch.ElapsedMilliseconds;
			if (elapsedMilliseconds != 0L)
			{
				Console.WriteLine(string.Concat(new object[]
				{
					DateTime.Now,
					" Data run done, count = ",
					this.data_count,
					" ms = ",
					this.watch.ElapsedMilliseconds,
					" event/sec = ",
					this.data_count / elapsedMilliseconds * 1000L
				}));
			}
			else
			{
				Console.WriteLine(string.Concat(new object[]
				{
					DateTime.Now,
					" Data run done, count = ",
					this.data_count,
					" ms = 0"
				}));
			}
			this.framework.Mode = FrameworkMode.Realtime;
		}
		private void OnSimulatorProgress(Event e)
		{
			OnSimulatorProgress arg_06_0 = (OnSimulatorProgress)e;
		}
		private void OnBid(Event e)
		{
			this.data_count += 1L;
			Bid bid = (Bid)e;
			if (this.framework.Clock.Mode == ClockMode.Simulation)
			{
				this.framework.Clock.DateTime = bid.dateTime;
			}
			else
			{
				bid.dateTime = this.framework.clock.DateTime;
			}
			this.factory.OnData(bid);
			this.framework.dataManager.OnBid(bid);
			this.framework.instrumentManager.GetById(bid.instrumentId).bid = bid;
			this.framework.providerManager.executionSimulator.OnBid(bid);
			this.framework.strategyManager.OnBid(bid);
		}
		private void OnAsk(Event e)
		{
			this.data_count += 1L;
			Ask ask = (Ask)e;
			if (this.framework.Clock.Mode == ClockMode.Simulation)
			{
				this.framework.Clock.DateTime = ask.DateTime;
			}
			else
			{
				ask.dateTime = this.framework.clock.DateTime;
			}
			this.factory.OnData(ask);
			this.framework.dataManager.OnAsk(ask);
			this.framework.instrumentManager.GetById(ask.instrumentId).ask = ask;
			this.framework.providerManager.executionSimulator.OnAsk(ask);
			this.framework.strategyManager.OnAsk(ask);
		}
		private void OnTrade(Event e)
		{
			this.data_count += 1L;
			Trade trade = (Trade)e;
			if (this.framework.Clock.Mode == ClockMode.Simulation)
			{
				this.framework.Clock.DateTime = trade.dateTime;
			}
			else
			{
				trade.dateTime = this.framework.clock.DateTime;
			}
			this.factory.OnData(trade);
			this.framework.dataManager.OnTrade(trade);
			this.framework.instrumentManager.GetById(trade.instrumentId).trade = trade;
			this.framework.providerManager.executionSimulator.OnTrade(trade);
			this.framework.strategyManager.OnTrade(trade);
		}
		private void OnBar(Event e)
		{
			this.data_count += 1L;
			Bar bar = (Bar)e;
			this.framework.dataManager.OnBar(bar);
			this.framework.providerManager.executionSimulator.OnBar(bar);
			this.framework.strategyManager.OnBar(bar);
		}
		private void OnLevel2Snapshot(Event e)
		{
			this.data_count += 1L;
			Level2Snapshot level2Snapshot = (Level2Snapshot)e;
			if (this.framework.Clock.Mode == ClockMode.Simulation)
			{
				this.framework.Clock.DateTime = level2Snapshot.dateTime;
			}
			else
			{
				level2Snapshot.dateTime = this.framework.Clock.DateTime;
			}
			this.framework.dataManager.OnLevel2Snapshot(level2Snapshot);
			this.framework.providerManager.executionSimulator.OnLevel2(level2Snapshot);
			this.framework.strategyManager.OnLevel2(level2Snapshot);
		}
		private void OnLevel2Update(Event e)
		{
			this.data_count += 1L;
			Level2Update level2Update = (Level2Update)e;
			if (this.framework.Clock.Mode == ClockMode.Simulation)
			{
				this.framework.Clock.DateTime = level2Update.dateTime;
			}
			else
			{
				level2Update.dateTime = this.framework.Clock.DateTime;
			}
			this.framework.dataManager.OnLevel2Update(level2Update);
			this.framework.providerManager.executionSimulator.OnLevel2(level2Update);
			this.framework.StrategyManager.OnLevel2(level2Update);
		}
		private void OnFundamental(Event e)
		{
			this.data_count += 1L;
			Fundamental fundamental = (Fundamental)e;
			if (this.framework.Clock.Mode == ClockMode.Simulation)
			{
				this.framework.Clock.DateTime = fundamental.dateTime;
			}
			else
			{
				fundamental.dateTime = this.framework.clock.DateTime;
			}
			this.framework.dataManager.OnFundamental(fundamental);
			this.framework.strategyManager.OnFundamental(fundamental);
		}
		private void OnNews(Event e)
		{
			this.data_count += 1L;
			News news = (News)e;
			if (this.framework.Clock.Mode == ClockMode.Simulation)
			{
				this.framework.Clock.DateTime = news.dateTime;
			}
			else
			{
				news.dateTime = this.framework.clock.DateTime;
			}
			this.framework.dataManager.OnNews(news);
			this.framework.strategyManager.OnNews(news);
		}
		private void OnExecutionReport(Event e)
		{
			ExecutionReport executionReport = (ExecutionReport)e;
			if (this.framework.Clock.Mode == ClockMode.Realtime)
			{
				executionReport.dateTime = this.framework.Clock.DateTime;
			}
			this.framework.orderManager.OnExecutionReport(executionReport);
			this.framework.portfolioManager.OnExecutionReport(executionReport);
			this.framework.strategyManager.OnExecutionReport(executionReport);
		}
		private void OnAccountData(Event e)
		{
			AccountData data = (AccountData)e;
			this.framework.accountDataManager.OnAccountData(data);
		}
		private void OnOrderStatusChanged(Event e)
		{
			this.framework.strategyManager.OnOrderStatusChanged(((OnOrderStatusChanged)e).order);
		}
		private void OnOrderPartiallyFilled(Event e)
		{
			this.framework.strategyManager.OnOrderPartiallyFilled(((OnOrderPartiallyFilled)e).order);
		}
		private void OnOrderFilled(Event e)
		{
			this.framework.strategyManager.OnOrderFilled(((OnOrderFilled)e).order);
		}
		private void OnOrderReplaced(Event e)
		{
			this.framework.strategyManager.OnOrderReplaced(((OnOrderReplaced)e).order);
		}
		private void OnOrderCancelled(Event e)
		{
			this.framework.strategyManager.OnOrderCancelled(((OnOrderCancelled)e).order);
		}
		private void OnOrderDone(Event e)
		{
			this.framework.strategyManager.OnOrderDone(((OnOrderDone)e).order);
		}
		private void OnFill(Event e)
		{
			this.framework.strategyManager.OnFill((OnFill)e);
		}
		private void OnPositionOpened(Event e)
		{
			OnPositionOpened onPositionOpened = (OnPositionOpened)e;
			this.framework.strategyManager.OnPositionOpened(onPositionOpened.portfolio, onPositionOpened.position);
		}
		private void OnPositionClosed(Event e)
		{
			OnPositionClosed onPositionClosed = (OnPositionClosed)e;
			this.framework.strategyManager.OnPositionClosed(onPositionClosed.portfolio, onPositionClosed.position);
		}
		private void OnPositionChanged(Event e)
		{
			OnPositionChanged onPositionChanged = (OnPositionChanged)e;
			this.framework.strategyManager.OnPositionChanged(onPositionChanged.portfolio, onPositionChanged.position);
		}
		private void OnReminder(Event e)
		{
			if (this.framework.Clock.Mode == ClockMode.Simulation)
			{
				this.framework.Clock.DateTime = e.dateTime;
			}
			((Reminder)e).Execute();
		}
		private void OnGroup(Event e)
		{
			this.framework.groupManager.OnGroup((Group)e);
		}
		private void OnGroupEvent(Event e)
		{
			this.framework.groupManager.OnGroupEvent((GroupEvent)e);
		}
		private void OnHistoricalData(Event e)
		{
			this.framework.dataManager.OnHistoricalData((HistoricalData)e);
		}
		private void OnHistoricalDataEnd(Event e)
		{
			this.framework.dataManager.OnHistoricalDataEnd((HistoricalDataEnd)e);
		}
		public void Clear()
		{
			this.count = 0L;
			this.data_count = 0L;
			this.factory.Clear();
		}
	}
}
