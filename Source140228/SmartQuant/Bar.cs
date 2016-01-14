using System;
namespace SmartQuant
{
	public class Bar : DataObject
	{
		internal int instrumentId;
		internal BarType type = BarType.Time;
		internal long size;
		internal DateTime openDateTime = DateTime.MinValue;
		internal double high;
		internal double low;
		internal double open;
		internal double close = double.NaN;
		internal long volume;
		internal long openInt;
		internal long n;
		internal double mean;
		internal double variance;
		internal bool isComplete;
		internal IdArray<double> fields;
		internal static BarFieldByName fieldByName = new BarFieldByName();
		public override byte TypeId
		{
			get
			{
				return 6;
			}
		}
		public BarType Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}
		public DateTime CloseDateTime
		{
			get
			{
				return this.dateTime;
			}
		}
		public DateTime OpenDateTime
		{
			get
			{
				return this.openDateTime;
			}
		}
		public TimeSpan Duration
		{
			get
			{
				return this.CloseDateTime - this.OpenDateTime;
			}
		}
		public int InstrumentId
		{
			get
			{
				return this.instrumentId;
			}
			set
			{
				this.instrumentId = value;
			}
		}
		public bool IsComplete
		{
			get
			{
				return this.isComplete;
			}
		}
		public double Open
		{
			get
			{
				return this.open;
			}
			set
			{
				this.open = value;
			}
		}
		public double High
		{
			get
			{
				return this.high;
			}
			set
			{
				this.high = value;
			}
		}
		public double Low
		{
			get
			{
				return this.low;
			}
			set
			{
				this.low = value;
			}
		}
		public double Close
		{
			get
			{
				return this.close;
			}
			set
			{
				this.close = value;
			}
		}
		public long Volume
		{
			get
			{
				return this.volume;
			}
			set
			{
				this.volume = value;
			}
		}
		public long OpenInt
		{
			get
			{
				return this.openInt;
			}
			set
			{
				this.openInt = value;
			}
		}
		public long N
		{
			get
			{
				return this.n;
			}
			set
			{
				this.n = value;
			}
		}
		public long Size
		{
			get
			{
				return this.size;
			}
			set
			{
				this.size = value;
			}
		}
		public double Mean
		{
			get
			{
				return this.mean;
			}
			set
			{
				this.mean = value;
			}
		}
		public double Variance
		{
			get
			{
				return this.variance;
			}
			set
			{
				this.variance = value;
			}
		}
		public double StdDev
		{
			get
			{
				return Math.Sqrt(this.variance);
			}
		}
		public double Range
		{
			get
			{
				return this.High - this.Low;
			}
		}
		public double Median
		{
			get
			{
				return (this.High + this.Low) / 2.0;
			}
		}
		public double Typical
		{
			get
			{
				return (this.High + this.Low + this.Close) / 3.0;
			}
		}
		public double Weighted
		{
			get
			{
				return (this.High + this.Low + 2.0 * this.Close) / 4.0;
			}
		}
		public double Average
		{
			get
			{
				return (this.Open + this.High + this.Low + this.Close) / 4.0;
			}
		}
		public double this[byte index]
		{
			get
			{
				return this.fields[(int)index];
			}
			set
			{
				if (this.fields == null)
				{
					this.fields = new IdArray<double>(10);
				}
				this.fields[(int)index] = value;
			}
		}
		public double this[string name]
		{
			get
			{
				return this.fields[(int)Bar.fieldByName[name]];
			}
			set
			{
				this[Bar.fieldByName[name]] = value;
			}
		}
		public static void AddField(string name, byte index)
		{
			Bar.fieldByName.Add(name, index);
		}
		public Bar(DateTime openDateTime, DateTime closeDateTime, int instrumentId, BarType type, long size, double open = 0.0, double high = 0.0, double low = 0.0, double close = 0.0, long volume = 0L, long openInt = 0L) : base(closeDateTime)
		{
			this.openDateTime = openDateTime;
			this.instrumentId = instrumentId;
			this.type = type;
			this.size = size;
			this.open = open;
			this.high = high;
			this.low = low;
			this.close = close;
			this.volume = volume;
		}
		public Bar()
		{
		}
		public Bar(Bar bar) : base(bar)
		{
			this.instrumentId = bar.instrumentId;
			this.type = bar.type;
			this.size = bar.size;
			this.openDateTime = bar.openDateTime;
			this.open = bar.open;
			this.high = bar.high;
			this.low = bar.low;
			this.close = bar.close;
			this.volume = bar.volume;
			this.openInt = bar.openInt;
		}
		public override string ToString()
		{
			return string.Format("Bar [{0} - {1}] Instrument={2} Type={3} Size={4} Open={5} High={6} Low={7} Close={8} Volume={9}", new object[]
			{
				this.openDateTime,
				this.dateTime,
				this.instrumentId,
				this.type,
				this.size,
				this.open,
				this.high,
				this.low,
				this.close,
				this.volume
			});
		}
	}
}
