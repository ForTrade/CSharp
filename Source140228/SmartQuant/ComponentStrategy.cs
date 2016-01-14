using System;
namespace SmartQuant
{
	public class ComponentStrategy : InstrumentStrategy
	{
		private DataComponent dataComponent;
		private AlphaComponent alphaComponent;
		private PositionComponent positionComponent;
		private RiskComponent riskComponent;
		private ExecutionComponent executionComponent;
		private ReportComponent reportComponent;
		public DataComponent DataComponent
		{
			get
			{
				return this.dataComponent;
			}
			set
			{
				this.dataComponent = value;
				this.dataComponent.strategy = this;
				this.dataComponent.framework = this.framework;
			}
		}
		public AlphaComponent AlphaComponent
		{
			get
			{
				return this.alphaComponent;
			}
			set
			{
				this.alphaComponent = value;
				this.alphaComponent.strategy = this;
				this.alphaComponent.framework = this.framework;
			}
		}
		public PositionComponent PositionComponent
		{
			get
			{
				return this.positionComponent;
			}
			set
			{
				this.positionComponent = value;
				this.positionComponent.strategy = this;
				this.positionComponent.framework = this.framework;
			}
		}
		public RiskComponent RiskComponent
		{
			get
			{
				return this.riskComponent;
			}
			set
			{
				this.riskComponent = value;
				this.riskComponent.strategy = this;
				this.riskComponent.framework = this.framework;
			}
		}
		public ExecutionComponent ExecutionComponent
		{
			get
			{
				return this.executionComponent;
			}
			set
			{
				this.executionComponent = value;
				this.executionComponent.strategy = this;
				this.executionComponent.framework = this.framework;
			}
		}
		public ReportComponent ReportComponent
		{
			get
			{
				return this.reportComponent;
			}
			set
			{
				this.reportComponent = value;
				this.reportComponent.strategy = this;
				this.reportComponent.framework = this.framework;
			}
		}
		public ComponentStrategy(Framework framework, string name) : base(framework, name)
		{
			this.DataComponent = new DataComponent();
			this.AlphaComponent = new AlphaComponent();
			this.PositionComponent = new PositionComponent();
			this.RiskComponent = new RiskComponent();
			this.ExecutionComponent = new ExecutionComponent();
			this.ReportComponent = new ReportComponent();
		}
		protected internal override void OnStrategyStart()
		{
			this.dataComponent.OnStrategyStart();
			this.alphaComponent.OnStrategyStart();
			this.positionComponent.OnStrategyStart();
			this.riskComponent.OnStrategyStart();
			this.executionComponent.OnStrategyStart();
			this.reportComponent.OnStrategyStart();
		}
		protected internal override void OnBar(Instrument instrument, Bar bar)
		{
			this.dataComponent.OnBar(bar);
			this.alphaComponent.OnBar(bar);
			this.positionComponent.OnBar(bar);
			this.reportComponent.OnBar(bar);
		}
		protected internal override void OnTrade(Instrument instrument, Trade trade)
		{
			this.alphaComponent.OnTrade(trade);
			this.positionComponent.OnTrade(trade);
		}
		protected internal override void OnBid(Instrument instrument, Bid bid)
		{
			this.alphaComponent.OnBid(bid);
			this.positionComponent.OnBid(bid);
		}
		protected internal override void OnAsk(Instrument instrument, Ask ask)
		{
			this.alphaComponent.OnAsk(ask);
			this.positionComponent.OnAsk(ask);
		}
		protected internal override void OnFill(Fill fill)
		{
			this.reportComponent.OnFill(fill);
		}
		protected internal override void OnPositionOpened(Position position)
		{
			this.positionComponent.OnPositionOpened(position);
			this.riskComponent.OnPositionOpened(position);
		}
		protected internal override void OnPositionClosed(Position position)
		{
			this.positionComponent.OnPositionClosed(position);
			this.riskComponent.OnPositionClosed(position);
		}
		protected internal override void OnPositionChanged(Position position)
		{
			this.positionComponent.OnPositionChanged(position);
			this.riskComponent.OnPositionChanged(position);
		}
		protected internal override void OnExecutionReport(ExecutionReport report)
		{
			this.executionComponent.OnExecutionReport(report);
		}
		protected internal override void OnOrderFilled(Order order)
		{
			this.executionComponent.OnOrderFilled(order);
		}
		protected internal override void OnStopExecuted(Stop stop)
		{
			this.positionComponent.OnStopExecuted(stop);
		}
		protected internal override void OnStopCancelled(Stop stop)
		{
			this.positionComponent.OnStopCancelled(stop);
		}
	}
}
