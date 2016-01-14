using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
namespace SmartQuant
{
	public class Framework : IDisposable
	{
		private static Framework currentFramework;
		private string name;
		private FrameworkMode mode = FrameworkMode.Realtime;
		private bool disposed;
		internal bool isExternalDataQueue;
		internal Configuration configuration;
		internal Clock clock;
		internal EventBus eventBus;
		internal EventServer eventServer;
		internal EventManager eventManager;
		internal InstrumentServer instrumentServer;
		internal InstrumentManager instrumentManager;
		internal DataServer dataServer;
		internal DataManager dataManager;
		internal ProviderManager providerManager;
		internal EventLoggerManager eventLoggerManager;
		internal SubscriptionManager subscriptionManager;
		internal OrderManager orderManager;
		internal PortfolioManager portfolioManager;
		internal StrategyManager strategyManager;
		internal GroupManager groupManager;
		internal GroupDispatcher groupDispatcher;
		internal StreamerManager streamerManager;
		internal DataFileManager dataFileManager;
		internal AccountDataManager accountDataManager;
		internal ICurrencyConverter currencyConverter;
		public bool IsExternalDataQueue
		{
			get
			{
				return this.isExternalDataQueue;
			}
			set
			{
				this.isExternalDataQueue = value;
			}
		}
		public string Name
		{
			get
			{
				return this.name;
			}
		}
		public Configuration Configuration
		{
			get
			{
				return this.configuration;
			}
		}
		public FrameworkMode Mode
		{
			get
			{
				return this.mode;
			}
			set
			{
				if (value != this.mode)
				{
					this.mode = value;
					switch (this.mode)
					{
					case FrameworkMode.Simulation:
						this.clock.Mode = ClockMode.Simulation;
						if (this.eventBus != null)
						{
							this.eventBus.Mode = EventBusMode.Simulation;
							return;
						}
						break;
					case FrameworkMode.Realtime:
						this.clock.Mode = ClockMode.Realtime;
						if (this.eventBus != null)
						{
							this.eventBus.Mode = EventBusMode.Realtime;
						}
						break;
					default:
						return;
					}
				}
			}
		}
		public Clock Clock
		{
			get
			{
				return this.clock;
			}
		}
		public EventBus EventBus
		{
			get
			{
				return this.eventBus;
			}
		}
		public EventServer EventServer
		{
			get
			{
				return this.eventServer;
			}
		}
		public EventManager EventManager
		{
			get
			{
				return this.eventManager;
			}
		}
		public InstrumentServer InstrumentServer
		{
			get
			{
				return this.instrumentServer;
			}
		}
		public InstrumentManager InstrumentManager
		{
			get
			{
				return this.instrumentManager;
			}
			set
			{
				this.instrumentManager = value;
			}
		}
		public DataServer DataServer
		{
			get
			{
				return this.dataServer;
			}
		}
		public DataManager DataManager
		{
			get
			{
				return this.dataManager;
			}
			set
			{
				this.dataManager = value;
			}
		}
		public ProviderManager ProviderManager
		{
			get
			{
				return this.providerManager;
			}
		}
		public EventLoggerManager EventLoggerManager
		{
			get
			{
				return this.eventLoggerManager;
			}
		}
		public SubscriptionManager SubscriptionManager
		{
			get
			{
				return this.subscriptionManager;
			}
		}
		public OrderManager OrderManager
		{
			get
			{
				return this.orderManager;
			}
		}
		public PortfolioManager PortfolioManager
		{
			get
			{
				return this.portfolioManager;
			}
		}
		public StrategyManager StrategyManager
		{
			get
			{
				return this.strategyManager;
			}
		}
		public GroupManager GroupManager
		{
			get
			{
				return this.groupManager;
			}
		}
		public AccountDataManager AccountDataManager
		{
			get
			{
				return this.accountDataManager;
			}
		}
		public ICurrencyConverter CurrencyConverter
		{
			get
			{
				return this.currencyConverter;
			}
			set
			{
				this.currencyConverter = value;
			}
		}
		public GroupDispatcher GroupDispatcher
		{
			get
			{
				return this.groupDispatcher;
			}
			set
			{
				this.groupDispatcher = value;
			}
		}
		public StreamerManager StreamerManager
		{
			get
			{
				return this.StreamerManager;
			}
		}
		public DataFileManager DataFileManager
		{
			get
			{
				return this.dataFileManager;
			}
		}
		public static Framework Current
		{
			get
			{
				if (Framework.currentFramework == null)
				{
					Framework.currentFramework = new Framework("", true);
				}
				return Framework.currentFramework;
			}
		}
		public IExecutionProvider ExecutionProvider
		{
			get
			{
				return (IExecutionProvider)this.providerManager.GetProvider(this.Configuration.DefaultExecutionProvider);
			}
		}
		public IDataProvider DataProvider
		{
			get
			{
				return (IDataProvider)this.providerManager.GetProvider(this.Configuration.DefaultDataProvider);
			}
		}
		public Framework(string name = "", bool createServers = true)
		{
			this.name = name;
			this.LoadConfiguration();
			this.eventBus = new EventBus(this, EventBusMode.Realtime);
			this.clock = new Clock(this, ClockMode.Realtime, false);
			this.eventBus.reminderQueue = this.clock.reminderQueue;
			this.eventServer = new EventServer(this, this.eventBus);
			this.eventManager = new EventManager(this, this.eventBus);
			this.streamerManager = new StreamerManager();
			this.streamerManager.Add(new DataObjectStreamer());
			this.streamerManager.Add(new InstrumentStreamer());
			this.streamerManager.Add(new BidStreamer());
			this.streamerManager.Add(new AskStreamer());
			this.streamerManager.Add(new QuoteStreamer());
			this.streamerManager.Add(new TradeStreamer());
			this.streamerManager.Add(new BarStreamer());
			this.streamerManager.Add(new Level2SnapshotStreamer());
			this.streamerManager.Add(new Level2UpdateStreamer());
			this.streamerManager.Add(new NewsStreamer());
			this.streamerManager.Add(new FundamentalStreamer());
			this.streamerManager.Add(new DataSeriesStreamer());
			if (createServers)
			{
				if (this.configuration.IsInstrumentFileLocal)
				{
					this.instrumentServer = new FileInstrumentServer(this, this.configuration.InstrumentFileName, null);
				}
				else
				{
					this.instrumentServer = new FileInstrumentServer(this, "instruments.quant", this.configuration.InstrumentFileHost);
				}
				this.instrumentManager = new InstrumentManager(this, this.InstrumentServer);
				if (this.configuration.IsDataFileLocal)
				{
					this.dataServer = new FileDataServer(this, this.configuration.DataFileName, null);
				}
				else
				{
					this.dataServer = new FileDataServer(this, "data.quant", this.configuration.DataFileHost);
				}
				this.dataManager = new DataManager(this, this.dataServer);
			}
			this.providerManager = new ProviderManager(this, null, new ExecutionSimulator(this));
			this.eventLoggerManager = new EventLoggerManager();
			this.subscriptionManager = new SubscriptionManager(this);
			this.orderManager = new OrderManager(this);
			this.portfolioManager = new PortfolioManager(this);
			this.strategyManager = new StrategyManager(this);
			this.groupManager = new GroupManager(this);
			this.accountDataManager = new AccountDataManager(this);
			this.currencyConverter = new CurrencyConverter(this);
			this.dataFileManager = new DataFileManager(Installation.DataDir.FullName);
			if (Framework.currentFramework == null)
			{
				Framework.currentFramework = this;
			}
		}
		public Framework(string name, InstrumentServer instrumentServer, DataServer dataServer)
		{
			this.name = name;
			this.LoadConfiguration();
			this.clock = new Clock(this, ClockMode.Realtime, false);
			this.eventBus = new EventBus(this, EventBusMode.Realtime);
			this.eventBus.reminderQueue = this.clock.reminderQueue;
			this.eventServer = new EventServer(this, this.eventBus);
			this.eventManager = new EventManager(this, this.eventBus);
			this.streamerManager = new StreamerManager();
			this.streamerManager.Add(new DataObjectStreamer());
			this.streamerManager.Add(new InstrumentStreamer());
			this.streamerManager.Add(new BidStreamer());
			this.streamerManager.Add(new AskStreamer());
			this.streamerManager.Add(new QuoteStreamer());
			this.streamerManager.Add(new TradeStreamer());
			this.streamerManager.Add(new BarStreamer());
			this.streamerManager.Add(new Level2SnapshotStreamer());
			this.streamerManager.Add(new Level2UpdateStreamer());
			this.streamerManager.Add(new NewsStreamer());
			this.streamerManager.Add(new FundamentalStreamer());
			this.streamerManager.Add(new DataSeriesStreamer());
			this.instrumentServer = instrumentServer;
			this.instrumentManager = new InstrumentManager(this, this.InstrumentServer);
			this.dataServer = dataServer;
			this.dataManager = new DataManager(this, dataServer);
			this.providerManager = new ProviderManager(this, null, new ExecutionSimulator(this));
			this.providerManager.AddProvider(new MatchingEngine(this));
			this.eventLoggerManager = new EventLoggerManager();
			this.subscriptionManager = new SubscriptionManager(this);
			this.orderManager = new OrderManager(this);
			this.portfolioManager = new PortfolioManager(this);
			this.strategyManager = new StrategyManager(this);
			this.groupManager = new GroupManager(this);
			this.currencyConverter = new CurrencyConverter(this);
			this.dataFileManager = new DataFileManager(Installation.DataDir.FullName);
			if (Framework.currentFramework == null)
			{
				Framework.currentFramework = this;
			}
		}
		public Framework(string name, EventBus externalBus, InstrumentServer instrumentServer, DataServer dataServer = null)
		{
			this.isExternalDataQueue = true;
			this.name = name;
			this.LoadConfiguration();
			this.clock = new Clock(this, ClockMode.Realtime, false);
			this.eventBus = new EventBus(this, EventBusMode.Realtime);
			this.eventBus.reminderQueue = this.clock.reminderQueue;
			externalBus.Attach(this.eventBus);
			this.eventServer = new EventServer(this, this.eventBus);
			this.eventManager = new EventManager(this, this.eventBus);
			this.streamerManager = new StreamerManager();
			this.streamerManager.Add(new DataObjectStreamer());
			this.streamerManager.Add(new InstrumentStreamer());
			this.streamerManager.Add(new BidStreamer());
			this.streamerManager.Add(new AskStreamer());
			this.streamerManager.Add(new QuoteStreamer());
			this.streamerManager.Add(new TradeStreamer());
			this.streamerManager.Add(new BarStreamer());
			this.streamerManager.Add(new Level2SnapshotStreamer());
			this.streamerManager.Add(new Level2UpdateStreamer());
			this.streamerManager.Add(new NewsStreamer());
			this.streamerManager.Add(new FundamentalStreamer());
			this.streamerManager.Add(new DataSeriesStreamer());
			this.instrumentServer = instrumentServer;
			this.instrumentManager = new InstrumentManager(this, instrumentServer);
			this.dataServer = dataServer;
			this.dataManager = new DataManager(this, dataServer);
			this.providerManager = new ProviderManager(this, null, new ExecutionSimulator(this));
			this.eventLoggerManager = new EventLoggerManager();
			this.orderManager = new OrderManager(this);
			this.portfolioManager = new PortfolioManager(this);
			this.strategyManager = new StrategyManager(this);
			this.groupManager = new GroupManager(this);
			this.currencyConverter = new CurrencyConverter(this);
			this.dataFileManager = new DataFileManager(Installation.DataDir.FullName);
			if (Framework.currentFramework == null)
			{
				Framework.currentFramework = this;
			}
		}
		private void LoadConfiguration()
		{
			string text;
			try
			{
				text = File.ReadAllText(Installation.ConfigDir.FullName + "\\configuration.xml");
			}
			catch (Exception)
			{
				text = null;
			}
			if (text != null)
			{
				using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(text)))
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));
					this.configuration = (Configuration)xmlSerializer.Deserialize(memoryStream);
					return;
				}
			}
			this.configuration = new Configuration();
		}
		private void SaveConfiguration()
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));
			StreamWriter streamWriter = new StreamWriter(Installation.ConfigDir.FullName + "\\configuration.xml");
			xmlSerializer.Serialize(streamWriter, this.configuration);
			streamWriter.Close();
		}
		public void Clear()
		{
			Console.WriteLine(DateTime.Now + " Framework::Clear");
			if (this.clock != null)
			{
				this.clock.Clear();
			}
			if (this.eventBus != null)
			{
				this.eventBus.Clear();
			}
			if (this.eventManager != null)
			{
				this.eventManager.Clear();
			}
			if (this.providerManager != null)
			{
				this.providerManager.Clear();
			}
			if (this.InstrumentManager != null)
			{
				this.InstrumentManager.Clear();
			}
			if (this.dataManager != null)
			{
				this.dataManager.Clear();
			}
			if (this.subscriptionManager != null)
			{
				this.subscriptionManager.Clear();
			}
			if (this.orderManager != null)
			{
				this.orderManager.Clear();
			}
			if (this.portfolioManager != null)
			{
				this.portfolioManager.Clear();
			}
			if (this.strategyManager != null)
			{
				this.strategyManager.Clear();
			}
			if (this.accountDataManager != null)
			{
				this.accountDataManager.Clear();
			}
			GC.Collect();
			this.eventServer.OnFrameworkCleared(this);
		}
		public void Dispose()
		{
			Console.WriteLine("Framework::Dispose " + this.name);
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.SaveConfiguration();
					this.instrumentServer.Dispose();
					this.dataServer.Dispose();
				}
				this.disposed = true;
			}
		}
		~Framework()
		{
			this.Dispose(false);
		}
	}
}
