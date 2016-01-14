using SmartQuant.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
namespace SmartQuant
{
	public class Instrument
	{
		private const string CATEGORY_APPEARANCE = "Appearance";
		private const string CATEGORY_DERIVATIVE = "Derivative";
		private const string CATEGORY_MARGIN = "Margin";
		private const string CATEGORY_INDUSTRY = "Industry";
		private const string CATEGORY_DISPLAY = "Display";
		private const string CATEGORY_TICK_SIZE = "TickSize";
		internal int id;
		internal InstrumentType type;
		internal string symbol;
		internal string description;
		internal string exchange = "";
		internal byte currencyId;
		internal double tickSize;
		internal DateTime maturity;
		internal double factor;
		internal double strike;
		internal PutCall putcall;
		internal double margin;
		internal List<Leg> legs;
		internal AltIdList altId = new AltIdList();
		internal Trade trade;
		internal Ask ask;
		internal Bid bid;
		internal IdArray<double> fields;
		internal Instrument parent;
		internal IDataProvider dataProvider;
		internal IExecutionProvider executionProvider;
		internal bool isPersistent;
		[Browsable(false)]
		public Instrument Parent
		{
			get
			{
				return this.parent;
			}
			set
			{
				this.parent = value;
			}
		}
		[Category("Appearance"), Description("Unique instrument id in SmartQuant framework")]
		public int Id
		{
			get
			{
				return this.id;
			}
			internal set
			{
				this.id = value;
			}
		}
		[Category("Appearance"), Description("Instrument Type (Stock, Futures, Option, Bond, ETF, Index, etc.)")]
		public InstrumentType Type
		{
			get
			{
				return this.type;
			}
		}
		[Category("Appearance"), Description("Instrument symbol")]
		public string Symbol
		{
			get
			{
				return this.symbol;
			}
		}
		[Category("Appearance"), Description("Instrument description")]
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}
		[Category("Appearance"), Description("Instrument exchange")]
		public string Exchange
		{
			get
			{
				return this.exchange;
			}
			set
			{
				this.exchange = value;
			}
		}
		[Category("Appearance"), Description("Instrument currency code (USD, EUR, RUR, CAD, etc.)")]
		public byte CurrencyId
		{
			get
			{
				return this.currencyId;
			}
			set
			{
				this.currencyId = value;
			}
		}
		[Category("TickSize"), DefaultValue(0.0), Description("Instrument tick size")]
		public double TickSize
		{
			get
			{
				return this.tickSize;
			}
			set
			{
				this.tickSize = value;
			}
		}
		[Category("Derivative"), Description("Instrument maturity")]
		public DateTime Maturity
		{
			get
			{
				return this.maturity;
			}
			set
			{
				this.maturity = value;
			}
		}
		[Category("Derivative"), DefaultValue(0.0), Description("Contract Value Factor by which price must be adjusted to determine the true nominal value of one futures/options contract. (Qty * Price) * Factor = Nominal Value")]
		public double Factor
		{
			get
			{
				return this.factor;
			}
			set
			{
				this.factor = value;
			}
		}
		[Category("Derivative"), DefaultValue(0.0), Description("Instrument strike price")]
		public double Strike
		{
			get
			{
				return this.strike;
			}
			set
			{
				this.strike = value;
			}
		}
		[Category("Derivative"), Description("Option type : put or call")]
		public PutCall PutCall
		{
			get
			{
				return this.putcall;
			}
			set
			{
				this.putcall = value;
			}
		}
		[Category("Margin"), DefaultValue(0.0), Description("Initial margin (used in simulations)")]
		public double Margin
		{
			get
			{
				return this.margin;
			}
			set
			{
				this.margin = value;
			}
		}
		[Editor(typeof(AltIdListEditor), typeof(UITypeEditor)), RefreshProperties(RefreshProperties.All), TypeConverter(typeof(AltIdListTypeConverter))]
		public AltIdList AltId
		{
			get
			{
				return this.altId;
			}
		}
		[Browsable(false)]
		public List<Leg> Legs
		{
			get
			{
				return this.legs;
			}
		}
		[Browsable(false)]
		public IdArray<double> Fields
		{
			get
			{
				return this.fields;
			}
		}
		[Browsable(false)]
		public Bid Bid
		{
			get
			{
				return this.bid;
			}
		}
		[Browsable(false)]
		public Ask Ask
		{
			get
			{
				return this.ask;
			}
		}
		[Browsable(false)]
		public Trade Trade
		{
			get
			{
				return this.trade;
			}
		}
		public IDataProvider DataProvider
		{
			get
			{
				return this.dataProvider;
			}
			set
			{
				this.dataProvider = value;
			}
		}
		public IExecutionProvider ExecutionProvider
		{
			get
			{
				return this.executionProvider;
			}
			set
			{
				this.executionProvider = value;
			}
		}
		public Instrument(Instrument instrument) : this()
		{
			this.id = instrument.id;
			this.type = instrument.type;
			this.symbol = instrument.symbol;
			this.description = instrument.description;
			this.exchange = instrument.exchange;
			this.currencyId = instrument.currencyId;
			this.tickSize = instrument.tickSize;
			this.putcall = instrument.putcall;
			this.factor = instrument.factor;
			this.strike = instrument.strike;
			this.maturity = instrument.maturity;
			this.margin = instrument.margin;
			foreach (Leg current in instrument.Legs)
			{
				this.Legs.Add(new Leg(current.Instrument, current.Weight));
			}
			this.trade = instrument.Trade;
			this.bid = instrument.Bid;
			this.ask = instrument.Ask;
		}
		internal Instrument(int id, InstrumentType type, string symbol, string description, byte currencyId = 1, string exchange = null) : this()
		{
			this.id = id;
			this.type = type;
			this.symbol = symbol;
			this.description = description;
			this.currencyId = currencyId;
			this.exchange = exchange;
		}
		public Instrument(InstrumentType type, string symbol, string description = "", byte currencyId = 1) : this()
		{
			this.type = type;
			this.symbol = symbol;
			this.description = description;
			this.currencyId = currencyId;
		}
		internal Instrument(int id, InstrumentType type, string symbol, string description = "", byte currencyId = 1) : this()
		{
			this.id = id;
			this.type = type;
			this.symbol = symbol;
			this.description = description;
			this.currencyId = currencyId;
		}
		private Instrument()
		{
			this.legs = new List<Leg>();
			this.fields = new IdArray<double>(10);
		}
		public string GetSymbol(byte providerId)
		{
			AltId altId = this.altId.Get(providerId);
			if (altId != null && !string.IsNullOrEmpty(altId.symbol))
			{
				return altId.symbol;
			}
			return this.symbol;
		}
		public string GetExchange(byte providerId)
		{
			AltId altId = this.altId.Get(providerId);
			if (altId != null && !string.IsNullOrEmpty(altId.exchange))
			{
				return altId.exchange;
			}
			return this.exchange;
		}
		public override string ToString()
		{
			if (string.IsNullOrEmpty(this.description))
			{
				return this.symbol;
			}
			return this.symbol + " (" + this.description + ")";
		}
		public Instrument Clone(string symbol = null)
		{
			Instrument instrument = new Instrument(this);
			if (symbol != null)
			{
				instrument.symbol = symbol;
			}
			return instrument;
		}
	}
}
