using System;
namespace SmartQuant
{
	public class PortfolioPerformance
	{
		internal Portfolio portfolio;
		internal DateTime dateTime;
		internal double equity;
		internal double drawdown;
		internal double highEquity;
		internal TimeSeries equitySeries = new TimeSeries("Equity", "Equity");
		internal TimeSeries drawdownSeries = new TimeSeries("Drawdown", "Drawdown");
		public event EventHandler Updated;
		public TimeSeries EquitySeries
		{
			get
			{
				return this.equitySeries;
			}
		}
		public TimeSeries DrawdownSeries
		{
			get
			{
				return this.drawdownSeries;
			}
		}
		internal PortfolioPerformance(Portfolio portfolio)
		{
			this.portfolio = portfolio;
		}
		public void Update()
		{
			this.dateTime = this.portfolio.framework.clock.DateTime;
			double value = this.portfolio.Value;
			if (this.equity == value)
			{
				return;
			}
			this.equity = value;
			if (this.equity > this.highEquity)
			{
				this.highEquity = this.equity;
				this.drawdown = 0.0;
			}
			else
			{
				this.drawdown = this.highEquity - this.equity;
			}
			this.equitySeries.Add(this.dateTime, this.equity);
			this.drawdownSeries.Add(this.dateTime, this.drawdown);
			if (this.Updated != null)
			{
				this.Updated(this, EventArgs.Empty);
			}
			if (this.portfolio.parent != null)
			{
				this.portfolio.parent.Performance.Update();
			}
		}
		public void Reset()
		{
			this.drawdown = 0.0;
			this.highEquity = -1.7976931348623157E+308;
		}
	}
}
