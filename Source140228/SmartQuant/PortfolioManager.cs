using System;
namespace SmartQuant
{
	public class PortfolioManager
	{
		private Framework framework;
		private PortfolioList portfolios = new PortfolioList();
		internal Pricer pricer;
		private int next_id;
		public Pricer Pricer
		{
			get
			{
				return this.pricer;
			}
			set
			{
				this.pricer = value;
			}
		}
		public PortfolioList Portfolios
		{
			get
			{
				return this.portfolios;
			}
		}
		public Portfolio this[int id]
		{
			get
			{
				return this.portfolios[id];
			}
		}
		public Portfolio this[string name]
		{
			get
			{
				return this.portfolios[name];
			}
		}
		public PortfolioManager(Framework framework)
		{
			this.framework = framework;
			this.pricer = new Pricer(framework);
		}
		public void Clear()
		{
			foreach (Portfolio current in this.portfolios)
			{
				this.framework.eventServer.OnPortfolioDeleted(current);
			}
			this.portfolios.Clear();
			this.next_id = 0;
		}
		public void Add(Portfolio portfolio)
		{
			this.Add(portfolio, true);
		}
		private void Add(Portfolio portfolio, bool emitEvent)
		{
			int id = this.next_id++;
			portfolio.id = id;
			this.portfolios.Add(portfolio);
			if (emitEvent)
			{
				this.framework.eventServer.OnPortfolioAdded(portfolio);
			}
		}
		public void Remove(string name)
		{
			Portfolio portfolio = this[name];
			if (portfolio != null)
			{
				this.Remove(portfolio);
				this.framework.eventServer.OnPortfolioDeleted(portfolio);
			}
		}
		public void Remove(int id)
		{
			Portfolio portfolio = this[id];
			if (portfolio != null)
			{
				this.Remove(portfolio);
			}
		}
		public void Remove(Portfolio portfolio)
		{
			this.portfolios.Remove(portfolio);
			this.framework.eventServer.OnPortfolioDeleted(portfolio);
		}
		internal void OnExecutionReport(ExecutionReport report)
		{
			if (report.execType == ExecType.ExecTrade)
			{
				Fill fill = new Fill(report);
				report.order.portfolio.Add(fill);
			}
		}
	}
}
