using System;
using System.Threading;
namespace SmartQuant
{
	public class EventDispatcher
	{
		protected internal Framework framework;
		private EventQueue queue;
		private Thread thread;
		public event FrameworkEventHandler FrameworkCleared;
		public event InstrumentEventHandler InstrumentAdded;
		public event InstrumentEventHandler InstrumentDeleted;
		public event InstrumentDefinitionEventHandler InstrumentDefinition;
		public event InstrumentDefinitionEndEventHandler InstrumentDefinitionEnd;
		public event ProviderEventHandler ProviderAdded;
		public event ProviderEventHandler ProviderRemoved;
		public event ProviderEventHandler ProviderStatusChanged;
		public event ProviderEventHandler ProviderConnected;
		public event ProviderEventHandler ProviderDisconnected;
		public event ExecutionCommandEventHandler ExecutionCommand;
		public event ExecutionReportEventHandler ExecutionReport;
		public event OrderManagerClearedEventHandler OrderManagerCleared;
		public event FillEventHandler Fill;
		public event BarEventHandler Bar;
		public event BidEventHandler Bid;
		public event AskEventHandler Ask;
		public event TradeEventHandler Trade;
		public event ProviderErrorEventHandler ProviderError;
		public event HistoricalDataEventHandler HistoricalData;
		public event HistoricalDataEndEventHandler HistoricalDataEnd;
		public event PortfolioEventHandler PortfolioAdded;
		public event PortfolioEventHandler PortfolioDeleted;
		public event PositionEventHandler PositionOpened;
		public event PositionEventHandler PositionChanged;
		public event PositionEventHandler PositionClosed;
		public event PortfolioEventHandler PortfolioParentChanged;
		public event GroupEventHandler NewGroup;
		public event GroupEventEventHandler NewGroupEvent;
		public event GroupUpdateEventHandler NewGroupUpdate;
		public event SimulatorProgressEventHandler SimulatorProgress;
		public event EventHandler SimulatorStop;
		public event AccountDataEventHandler AccountData;
		public event EventHandler EventManagerStarted;
		public event EventHandler EventManagerStopped;
		public event EventHandler EventManagerPaused;
		public event EventHandler EventManagerResumed;
		public event EventHandler EventManagerStep;
		public EventDispatcher(Framework framework)
		{
			this.framework = framework;
			if (this.queue != null)
			{
				this.thread = new Thread(new ThreadStart(this.ThreadRun));
				this.thread.IsBackground = true;
				this.thread.Name = "Event Dispatcher Thread";
				this.thread.Start();
			}
		}
		public void OnEvent(Event e)
		{
			if (this.queue != null)
			{
				this.queue.Enqueue(e);
				return;
			}
			this.OnEvent_(e);
		}
		private void ThreadRun()
		{
			Console.WriteLine("Event dispatcher thread started: Framework = " + this.framework.Name + " Clock = " + this.framework.Clock.GetModeAsString());
			while (true)
			{
				if (this.queue.IsEmpty())
				{
					Thread.Sleep(1);
				}
				else
				{
					this.OnEvent_(this.queue.Dequeue());
				}
			}
		}
		private void OnEvent_(Event e)
		{
			byte typeId = e.TypeId;
			if (typeId <= 21)
			{
				switch (typeId)
				{
				case 2:
					if (this.Bid != null)
					{
						this.Bid(this, (Bid)e);
						return;
					}
					break;
				case 3:
					if (this.Ask != null)
					{
						this.Ask(this, (Ask)e);
						return;
					}
					break;
				case 4:
					if (this.Trade != null)
					{
						this.Trade(this, (Trade)e);
						return;
					}
					break;
				case 5:
					break;
				case 6:
					if (this.Bar != null)
					{
						this.Bar(this, (Bar)e);
						return;
					}
					break;
				default:
					switch (typeId)
					{
					case 13:
						if (this.ExecutionReport != null)
						{
							this.ExecutionReport(this, (ExecutionReport)e);
							return;
						}
						break;
					case 14:
						if (this.ExecutionCommand != null)
						{
							this.ExecutionCommand(this, (ExecutionCommand)e);
							return;
						}
						break;
					default:
						if (typeId != 21)
						{
							return;
						}
						if (this.ProviderError != null)
						{
							this.ProviderError(this, new ProviderErrorEventArgs((ProviderError)e));
							return;
						}
						break;
					}
					break;
				}
			}
			else
			{
				switch (typeId)
				{
				case 50:
					if (this.NewGroup != null)
					{
						this.NewGroup(this, new GroupEventAgrs((Group)e));
						return;
					}
					break;
				case 51:
					if (this.NewGroupUpdate != null)
					{
						this.NewGroupUpdate(this, new GroupUpdateEventAgrs((GroupUpdate)e));
						return;
					}
					break;
				case 52:
					if (this.NewGroupEvent != null)
					{
						this.NewGroupEvent(this, new GroupEventEventAgrs((GroupEvent)e));
						return;
					}
					break;
				default:
					switch (typeId)
					{
					case 99:
						if (this.FrameworkCleared != null)
						{
							this.FrameworkCleared(this, new FrameworkEventArgs(((OnFrameworkCleared)e).framework));
							return;
						}
						break;
					case 100:
						if (this.InstrumentAdded != null)
						{
							this.InstrumentAdded(this, new InstrumentEventArgs(((OnInstrumentAdded)e).instrument));
							return;
						}
						break;
					case 101:
						if (this.InstrumentDeleted != null)
						{
							this.InstrumentDeleted(this, new InstrumentEventArgs(((OnInstrumentDeleted)e).instrument));
							return;
						}
						break;
					case 102:
						if (this.ProviderAdded != null)
						{
							this.ProviderAdded(this, new ProviderEventArgs(((OnProviderAdded)e).provider));
							return;
						}
						break;
					case 103:
						if (this.ProviderRemoved != null)
						{
							this.ProviderRemoved(this, new ProviderEventArgs(((OnProviderRemoved)e).provider));
							return;
						}
						break;
					case 104:
						if (this.ProviderConnected != null)
						{
							this.ProviderConnected(this, new ProviderEventArgs(((OnProviderConnected)e).provider));
							return;
						}
						break;
					case 105:
						if (this.ProviderDisconnected != null)
						{
							this.ProviderDisconnected(this, new ProviderEventArgs(((OnProviderDisconnected)e).provider));
							return;
						}
						break;
					case 106:
						if (this.ProviderStatusChanged != null)
						{
							this.ProviderStatusChanged(this, new ProviderEventArgs(((OnProviderStatusChanged)e).provider));
							return;
						}
						break;
					case 107:
					case 114:
					case 115:
					case 116:
					case 117:
					case 118:
					case 119:
					case 120:
					case 121:
					case 125:
					case 126:
					case 132:
					case 133:
					case 134:
					case 135:
					case 136:
					case 137:
					case 138:
					case 139:
						break;
					case 108:
						if (this.SimulatorStop != null)
						{
							this.SimulatorStop(this, EventArgs.Empty);
							return;
						}
						break;
					case 109:
						if (this.SimulatorProgress != null)
						{
							this.SimulatorProgress(this, new SimulatorProgressEventArgs(((OnSimulatorProgress)e).count, ((OnSimulatorProgress)e).percent));
							return;
						}
						break;
					case 110:
						if (this.PositionOpened != null)
						{
							this.PositionOpened(this, new PositionEventArgs(((OnPositionOpened)e).portfolio, ((OnPositionOpened)e).position));
							return;
						}
						break;
					case 111:
						if (this.PositionClosed != null)
						{
							this.PositionClosed(this, new PositionEventArgs(((OnPositionClosed)e).portfolio, ((OnPositionClosed)e).position));
							return;
						}
						break;
					case 112:
						if (this.PositionChanged != null)
						{
							this.PositionChanged(this, new PositionEventArgs(((OnPositionChanged)e).portfolio, ((OnPositionChanged)e).position));
							return;
						}
						break;
					case 113:
						if (this.Fill != null)
						{
							this.Fill(this, (OnFill)e);
							return;
						}
						break;
					case 122:
						if (this.OrderManagerCleared != null)
						{
							this.OrderManagerCleared(this, (OnOrderManagerCleared)e);
							return;
						}
						break;
					case 123:
						if (this.InstrumentDefinition != null)
						{
							this.InstrumentDefinition(this, new InstrumentDefinitionEventArgs(((OnInstrumentDefinition)e).definition));
							return;
						}
						break;
					case 124:
						if (this.InstrumentDefinitionEnd != null)
						{
							this.InstrumentDefinitionEnd(this, new InstrumentDefinitionEndEventArgs(((OnInstrumentDefinitionEnd)e).end));
							return;
						}
						break;
					case 127:
						if (this.PortfolioAdded != null)
						{
							this.PortfolioAdded(this, new PortfolioEventArgs(((OnPortfolioAdded)e).portfolio));
							return;
						}
						break;
					case 128:
						if (this.PortfolioDeleted != null)
						{
							this.PortfolioDeleted(this, new PortfolioEventArgs(((OnPortfolioDeleted)e).portfolio));
							return;
						}
						break;
					case 129:
						if (this.PortfolioParentChanged != null)
						{
							this.PortfolioParentChanged(this, new PortfolioEventArgs(((OnPortfolioParentChanged)e).portfolio));
							return;
						}
						break;
					case 130:
						if (this.HistoricalData != null)
						{
							this.HistoricalData(this, new HistoricalDataEventArgs((HistoricalData)e));
							return;
						}
						break;
					case 131:
						if (this.HistoricalDataEnd != null)
						{
							this.HistoricalDataEnd(this, new HistoricalDataEndEventArgs((HistoricalDataEnd)e));
							return;
						}
						break;
					case 140:
						if (this.AccountData != null)
						{
							this.AccountData(this, new AccountDataEventArgs((AccountData)e));
							return;
						}
						break;
					default:
						switch (typeId)
						{
						case 207:
							if (this.EventManagerStarted != null)
							{
								this.EventManagerStarted(this, EventArgs.Empty);
								return;
							}
							break;
						case 208:
							if (this.EventManagerStopped != null)
							{
								this.EventManagerStopped(this, EventArgs.Empty);
								return;
							}
							break;
						case 209:
							if (this.EventManagerPaused != null)
							{
								this.EventManagerPaused(this, EventArgs.Empty);
								return;
							}
							break;
						case 210:
							if (this.EventManagerResumed != null)
							{
								this.EventManagerResumed(this, EventArgs.Empty);
								return;
							}
							break;
						case 211:
							if (this.EventManagerStep != null)
							{
								this.EventManagerStep(this, EventArgs.Empty);
							}
							break;
						default:
							return;
						}
						break;
					}
					break;
				}
			}
		}
	}
}
