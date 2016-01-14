using System;
namespace SmartQuant
{
	public class EventServer
	{
		private Framework framework;
		internal EventBus bus;
		public EventServer(Framework framework, EventBus bus)
		{
			this.framework = framework;
			this.bus = bus;
		}
		public void OnInstrumentAdded(Instrument instrument)
		{
			this.OnEvent(new OnInstrumentAdded(instrument));
		}
		public void OnInstrumentDeleted(Instrument instrument)
		{
			this.OnEvent(new OnInstrumentDeleted(instrument));
		}
		public void OnInstrumentDefinition(InstrumentDefinition definition)
		{
			this.OnEvent(new OnInstrumentDefinition(definition));
		}
		public void OnInstrumentDefintionEnd(InstrumentDefinitionEnd end)
		{
			this.OnEvent(new OnInstrumentDefinitionEnd(end));
		}
		public void OnHistoricalData(HistoricalData data)
		{
			this.OnEvent(new OnHistoricalData(data));
		}
		public void OnHistoricalDataEnd(HistoricalDataEnd end)
		{
			this.OnEvent(new OnHistoricalDataEnd(end));
		}
		public void OnProviderAdded(IProvider provider)
		{
			this.OnEvent(new OnProviderAdded(provider));
		}
		public void OnProviderRemoved(Provider provider)
		{
			this.OnEvent(new OnProviderRemoved(provider));
		}
		public void OnProviderStatusChanged(Provider provider)
		{
			switch (provider.Status)
			{
			case ProviderStatus.Connected:
				this.OnProviderConnected(provider);
				break;
			case ProviderStatus.Disconnected:
				this.OnProviderDisconnected(provider);
				break;
			}
			this.OnEvent(new OnProviderStatusChanged(provider));
		}
		public void OnProviderError(ProviderError error)
		{
			this.framework.eventManager.OnEvent(error);
		}
		public void OnProviderConnected(Provider provider)
		{
			this.OnEvent(new OnProviderConnected(provider));
		}
		public void OnProviderDisconnected(Provider provider)
		{
			this.OnEvent(new OnProviderDisconnected(provider));
		}
		public void OnData(DataObject data)
		{
			this.OnEvent(data);
		}
		public void OnPortfolioAdded(Portfolio portfolio)
		{
			this.OnEvent(new OnPortfolioAdded(portfolio));
		}
		public void OnPortfolioDeleted(Portfolio portfolio)
		{
			this.OnEvent(new OnPortfolioDeleted(portfolio));
		}
		public void OnPositionOpened(Portfolio portfolio, Position position)
		{
			this.OnEvent(new OnPositionOpened(portfolio, position));
		}
		internal void OnPositionClosed(Portfolio portfolio, Position position)
		{
			this.OnEvent(new OnPositionClosed(portfolio, position));
		}
		internal void OnPositionChanged(Portfolio portfolio, Position position)
		{
			this.OnEvent(new OnPositionChanged(portfolio, position));
		}
		internal void OnFill(Portfolio portfolio, Fill fill)
		{
			this.OnEvent(new OnFill(portfolio, fill));
		}
		internal void OnParentChanged(Portfolio portfolio)
		{
			this.OnEvent(new OnPortfolioParentChanged(portfolio));
		}
		internal void OnExecutionCommand(ExecutionCommand command)
		{
			this.OnEvent(command);
		}
		public void OnLog(Event e)
		{
			this.OnEvent(e);
		}
		internal void OnResponse(Event e)
		{
			this.OnEvent(e);
		}
		internal void OnExecutionReport(ExecutionReport report)
		{
			this.OnEvent(report);
		}
		internal void OnOrderStatusChanged(Order order)
		{
			this.OnEvent(new OnOrderStatusChanged(order));
		}
		internal void OnOrderPartiallyFilled(Order order)
		{
			this.OnEvent(new OnOrderPartiallyFilled(order));
		}
		internal void OnOrderFilled(Order order)
		{
			this.OnEvent(new OnOrderFilled(order));
		}
		internal void OnOrderCancelled(Order order)
		{
			this.OnEvent(new OnOrderCancelled(order));
		}
		internal void OnOrderReplaced(Order order)
		{
			this.OnEvent(new OnOrderReplaced(order));
		}
		internal void OnOrderDone(Order order)
		{
			this.OnEvent(new OnOrderDone(order));
		}
		public void OnEvent(Event e)
		{
			this.framework.eventManager.OnEvent(e);
		}
		internal void OnOrderManagerCleared()
		{
			this.framework.eventManager.OnEvent(new OnOrderManagerCleared());
		}
		internal void OnFrameworkCleared(Framework framework)
		{
			framework.eventManager.OnEvent(new OnFrameworkCleared(framework));
		}
	}
}
