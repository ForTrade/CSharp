using System;
using System.Collections.Generic;
using System.ComponentModel;
namespace SmartQuant
{
	public class Portfolio
	{
		internal Framework framework;
		internal int id;
		internal string name;
		internal Account account;
		internal FillSeries fills;
		internal List<Position> positions;
		internal IdArray<Position> positionByInstrument;
		internal Pricer pricer;
		internal PortfolioPerformance performance;
		internal PortfolioStatistics statistics;
		internal Portfolio parent;
		public string Name
		{
			get
			{
				return this.name;
			}
		}
		[Browsable(false)]
		public Portfolio Parent
		{
			get
			{
				return this.parent;
			}
			set
			{
				if (this.parent != value)
				{
					this.parent = value;
					this.framework.eventServer.OnParentChanged(this);
				}
			}
		}
		[Browsable(false)]
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
		[Browsable(false)]
		public PortfolioPerformance Performance
		{
			get
			{
				return this.performance;
			}
		}
		[Browsable(false)]
		public PortfolioStatistics Statistics
		{
			get
			{
				return this.statistics;
			}
		}
		public double AccountValue
		{
			get
			{
				return this.account.Value;
			}
		}
		public double PositionValue
		{
			get
			{
				double num = 0.0;
				for (int i = 0; i < this.positions.Count; i++)
				{
					num += this.framework.currencyConverter.Convert(this.positions[i].Value, this.positions[i].instrument.currencyId, this.account.currencyId);
				}
				return num;
			}
		}
		[Browsable(false)]
		public List<Position> Positions
		{
			get
			{
				return this.positions;
			}
		}
		[Browsable(false)]
		public FillSeries Fills
		{
			get
			{
				return this.fills;
			}
		}
		[Browsable(false)]
		public Account Account
		{
			get
			{
				return this.account;
			}
		}
		public double Value
		{
			get
			{
				return this.AccountValue + this.PositionValue;
			}
		}
		public Portfolio(Framework framework, string name = "")
		{
			this.framework = framework;
			this.name = name;
			this.account = new Account(framework);
			this.fills = new FillSeries(name);
			this.positions = new List<Position>();
			this.positionByInstrument = new IdArray<Position>(1000);
			this.pricer = framework.portfolioManager.pricer;
			this.performance = new PortfolioPerformance(this);
			this.statistics = new PortfolioStatistics(this);
		}
		public void Add(Fill fill)
		{
			this.fills.Add(fill);
			Instrument instrument = fill.instrument;
			bool flag = false;
			Position position = this.positionByInstrument[instrument.Id];
			if (position == null)
			{
				position = new Position(this, instrument);
				this.positionByInstrument[instrument.Id] = position;
				this.positions.Add(position);
				flag = true;
			}
			if (position.qty == 0.0)
			{
				flag = true;
			}
			position.Add(fill);
			this.account.Add(fill);
			if (flag)
			{
				this.framework.eventServer.OnPositionOpened(this, position);
				this.framework.eventServer.OnPositionChanged(this, position);
			}
			else
			{
				this.framework.eventServer.OnPositionChanged(this, position);
				if (position.qty == 0.0)
				{
					this.framework.eventServer.OnPositionClosed(this, position);
				}
			}
			this.framework.eventServer.OnFill(this, fill);
			if (this.parent != null)
			{
				this.parent.Add(fill);
			}
			this.statistics.Add(fill);
		}
		public Position GetPosition(Instrument instrument)
		{
			return this.positionByInstrument[instrument.Id];
		}
		public bool HasPosition(Instrument instrument)
		{
			Position position = this.positionByInstrument[instrument.Id];
			return position != null && position.qty != 0.0;
		}
		public bool HasPosition(Instrument instrument, PositionSide side, double qty)
		{
			Position position = this.positionByInstrument[instrument.Id];
			return position != null && position.Side == side && position.qty == qty;
		}
		public bool HasLongPosition(Instrument instrument)
		{
			Position position = this.positionByInstrument[instrument.Id];
			return position != null && position.Side == PositionSide.Long && position.qty != 0.0;
		}
		public bool HasLongPosition(Instrument instrument, double qty)
		{
			Position position = this.positionByInstrument[instrument.Id];
			return position != null && position.Side == PositionSide.Long && position.qty == qty;
		}
		public bool HasShortPosition(Instrument instrument)
		{
			Position position = this.positionByInstrument[instrument.Id];
			return position != null && position.Side == PositionSide.Short && position.qty != 0.0;
		}
		public bool HasShortPosition(Instrument instrument, double qty)
		{
			Position position = this.positionByInstrument[instrument.Id];
			return position != null && position.Side == PositionSide.Short && position.qty == qty;
		}
	}
}
