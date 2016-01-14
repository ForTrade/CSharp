using System;
namespace SmartQuant
{
	public class SellSideStrategy : Strategy, IExecutionProvider, IDataProvider, IProvider
	{
		public new ProviderStatus Status
		{
			get;
			set;
		}
		public SellSideStrategy(Framework framework, string name) : base(framework, name)
		{
		}
		public virtual void Send(ExecutionCommand command)
		{
			switch (command.Type)
			{
			case ExecutionCommandType.Send:
				this.OnSendCommand(command);
				return;
			case ExecutionCommandType.Cancel:
				this.OnCancelCommand(command);
				return;
			case ExecutionCommandType.Replace:
				this.OnReplaceCommand(command);
				return;
			default:
				return;
			}
		}
		public virtual void OnSendCommand(ExecutionCommand command)
		{
		}
		public virtual void OnCancelCommand(ExecutionCommand command)
		{
		}
		public virtual void OnReplaceCommand(ExecutionCommand command)
		{
		}
		public void Connect()
		{
		}
		public void Disconnect()
		{
		}
		public void EmitExecutionReport(ExecutionReport report)
		{
			this.framework.eventManager.OnEvent(report);
		}
		public void EmitBid(Bid bid)
		{
			this.framework.eventManager.OnEvent(bid);
		}
		public void EmitAsk(Ask ask)
		{
			this.framework.eventManager.OnEvent(ask);
		}
		public void EmitTrade(Trade trade)
		{
			this.framework.eventManager.OnEvent(trade);
		}
		public virtual void Subscribe(Instrument instrument)
		{
			this.OnSubscribe(instrument);
		}
		public virtual void Subscribe(InstrumentList instruments)
		{
			this.OnSubscribe(instruments);
		}
		public virtual void Unsubscribe(Instrument instrument)
		{
			this.OnUnsubscribe(instrument);
		}
		public virtual void Unsubscribe(InstrumentList instruments)
		{
			this.OnUnsubscribe(instruments);
		}
		protected virtual void OnSubscribe(InstrumentList instruments)
		{
		}
		protected virtual void OnSubscribe(Instrument instrument)
		{
		}
		protected virtual void OnUnsubscribe(InstrumentList instruments)
		{
		}
		protected virtual void OnUnsubscribe(Instrument instrument)
		{
		}
	}
}
