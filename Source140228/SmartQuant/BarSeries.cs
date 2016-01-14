using System;
using System.Collections.Generic;
namespace SmartQuant
{
	public class BarSeries : ISeries
	{
		private List<Bar> items;
		private Bar min;
		private Bar max;
		protected string name;
		protected string description;
		internal List<Indicator> indicators;
		public string Name
		{
			get
			{
				return this.name;
			}
		}
		public string Description
		{
			get
			{
				return this.description;
			}
		}
		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}
		public List<Indicator> Indicators
		{
			get
			{
				return this.indicators;
			}
		}
		double ISeries.First
		{
			get
			{
				return this.First.Close;
			}
		}
		double ISeries.Last
		{
			get
			{
				return this.Last.Close;
			}
		}
		public DateTime FirstDateTime
		{
			get
			{
				if (this.Count <= 0)
				{
					throw new ApplicationException("Array has no elements");
				}
				return this.items[0].DateTime;
			}
		}
		public DateTime LastDateTime
		{
			get
			{
				if (this.Count <= 0)
				{
					throw new ApplicationException("Array has no elements");
				}
				return this.items[this.Count - 1].DateTime;
			}
		}
		public Bar First
		{
			get
			{
				if (this.Count <= 0)
				{
					throw new ApplicationException("Array has no elements");
				}
				return this[0];
			}
		}
		public Bar Last
		{
			get
			{
				if (this.Count <= 0)
				{
					throw new ApplicationException("Array has no elements");
				}
				return this[this.Count - 1];
			}
		}
		public Bar this[int index]
		{
			get
			{
				return this.items[index];
			}
		}
		public Bar this[DateTime dateTime, IndexOption option = IndexOption.Null]
		{
			get
			{
				return this.items[this.GetIndex(dateTime, option)];
			}
		}
		double ISeries.this[int index]
		{
			get
			{
				return this[index, BarData.Close];
			}
		}
		public double this[int index, BarData barData]
		{
			get
			{
				switch (barData)
				{
				case BarData.Close:
					return this.items[index].Close;
				case BarData.Open:
					return this.items[index].Open;
				case BarData.High:
					return this.items[index].High;
				case BarData.Low:
					return this.items[index].Low;
				case BarData.Median:
					return this.items[index].Median;
				case BarData.Typical:
					return this.items[index].Typical;
				case BarData.Weighted:
					return this.items[index].Weighted;
				case BarData.Average:
					return this.items[index].Average;
				case BarData.Volume:
					return (double)this.items[index].Volume;
				case BarData.OpenInt:
					return (double)this.items[index].OpenInt;
				case BarData.Range:
					return this.items[index].Range;
				case BarData.Mean:
					return this.items[index].Mean;
				case BarData.Variance:
					return this.items[index].Variance;
				case BarData.StdDev:
					return this.items[index].StdDev;
				default:
					throw new ArgumentException("Unknown BarData value " + barData);
				}
			}
		}
		public BarSeries(string name = "", string description = "")
		{
			this.name = name;
			this.description = description;
			this.items = new List<Bar>();
			this.indicators = new List<Indicator>();
			this.min = null;
			this.max = null;
		}
		public void Clear()
		{
			this.items.Clear();
			this.min = null;
			this.max = null;
			for (int i = 0; i < this.indicators.Count; i++)
			{
				this.indicators[i].Clear();
			}
			this.indicators.Clear();
		}
		public void Add(Bar bar)
		{
			if (this.min == null)
			{
				this.min = bar;
			}
			else
			{
				if (bar.high < this.min.low)
				{
					this.min = bar;
				}
			}
			if (this.max == null)
			{
				this.max = bar;
			}
			else
			{
				if (bar.high > this.max.high)
				{
					this.max = bar;
				}
			}
			this.items.Add(bar);
			int index = this.items.Count - 1;
			for (int i = 0; i < this.indicators.Count; i++)
			{
				this.indicators[i].Calculate(index);
			}
		}
		public Bar GetMin()
		{
			return this.min;
		}
		public Bar GetMax()
		{
			return this.max;
		}
		public double GetMin(DateTime dateTime1, DateTime dateTime2)
		{
			int index = this.GetIndex(dateTime1, IndexOption.Next);
			int index2 = this.GetIndex(dateTime2, IndexOption.Prev);
			if (index <= index2 && index != -1 && index2 != -1)
			{
				double num = 1.7976931348623157E+308;
				for (int i = index; i <= index2; i++)
				{
					Bar bar = this.items[i];
					if (num > bar.low)
					{
						num = bar.low;
					}
				}
				return num;
			}
			return double.NaN;
		}
		public double GetMin(int index1, int index2, BarData barData)
		{
			if (index1 <= index2 && index1 != -1 && index2 != -1)
			{
				double num = 1.7976931348623157E+308;
				for (int i = index1; i <= index2; i++)
				{
					double num2 = this[i, barData];
					if (num > num2)
					{
						num = num2;
					}
				}
				return num;
			}
			return double.NaN;
		}
		public double GetMax(DateTime dateTime1, DateTime dateTime2)
		{
			int index = this.GetIndex(dateTime1, IndexOption.Next);
			int index2 = this.GetIndex(dateTime2, IndexOption.Prev);
			if (index <= index2 && index != -1 && index2 != -1)
			{
				double num = -1.7976931348623157E+308;
				for (int i = index; i <= index2; i++)
				{
					Bar bar = this.items[i];
					if (num < bar.high)
					{
						num = bar.high;
					}
				}
				return num;
			}
			return double.NaN;
		}
		public double GetMax(int index1, int index2, BarData barData)
		{
			if (index1 <= index2 && index1 != -1 && index2 != -1)
			{
				double num = -1.7976931348623157E+308;
				for (int i = index1; i <= index2; i++)
				{
					double num2 = this[i, barData];
					if (num < num2)
					{
						num = num2;
					}
				}
				return num;
			}
			return double.NaN;
		}
		public Bar HighestHighBar(int index1, int index2)
		{
			if (this.Count == 0)
			{
				return null;
			}
			if (index1 > index2)
			{
				return null;
			}
			if (index1 < 0 || index1 > this.Count - 1)
			{
				return null;
			}
			if (index2 < 0 || index2 > this.Count - 1)
			{
				return null;
			}
			Bar bar = this.items[index1];
			for (int i = index1 + 1; i <= index2; i++)
			{
				if (this.items[i].High > bar.High)
				{
					bar = this.items[i];
				}
			}
			return bar;
		}
		public Bar HighestHighBar(int nBars)
		{
			return this.HighestHighBar(this.Count - nBars, this.Count - 1);
		}
		public Bar HighestHighBar(DateTime dateTime1, DateTime dateTime2)
		{
			int index = this.GetIndex(dateTime1, IndexOption.Next);
			int index2 = this.GetIndex(dateTime2, IndexOption.Prev);
			return this.HighestHighBar(index, index2);
		}
		public Bar HighestHighBar()
		{
			return this.max;
		}
		public double HighestHigh(int index1, int index2)
		{
			return this.HighestHighBar(index1, index2).High;
		}
		public double HighestHigh(int nBars)
		{
			return this.HighestHighBar(nBars).High;
		}
		public double HighestHigh(DateTime dateTime1, DateTime dateTime2)
		{
			return this.HighestHighBar(dateTime1, dateTime2).High;
		}
		public double HighestHigh()
		{
			return this.max.high;
		}
		public Bar HighestLowBar(int index1, int index2)
		{
			if (this.Count == 0)
			{
				return null;
			}
			if (index1 > index2)
			{
				return null;
			}
			if (index1 < 0 || index1 > this.Count - 1)
			{
				return null;
			}
			if (index2 < 0 || index2 > this.Count - 1)
			{
				return null;
			}
			Bar bar = this.items[index1];
			for (int i = index1 + 1; i <= index2; i++)
			{
				if (this.items[i].Low > bar.Low)
				{
					bar = this.items[i];
				}
			}
			return bar;
		}
		public Bar HighestLowBar(int nBars)
		{
			return this.HighestLowBar(this.Count - nBars, this.Count - 1);
		}
		public Bar HighestLowBar(DateTime dateTime1, DateTime dateTime2)
		{
			int index = this.GetIndex(dateTime1, IndexOption.Next);
			int index2 = this.GetIndex(dateTime2, IndexOption.Prev);
			return this.HighestLowBar(index, index2);
		}
		public double HighestLow(int index1, int index2)
		{
			return this.HighestLowBar(index1, index2).Low;
		}
		public double HighestLow(int nBars)
		{
			return this.HighestLowBar(nBars).Low;
		}
		public double HighestLow(DateTime dateTime1, DateTime dateTime2)
		{
			return this.HighestLowBar(dateTime1, dateTime2).Low;
		}
		public Bar LowestLowBar(int index1, int index2)
		{
			if (this.Count == 0)
			{
				return null;
			}
			if (index1 > index2)
			{
				return null;
			}
			if (index1 < 0 || index1 > this.Count - 1)
			{
				return null;
			}
			if (index2 < 0 || index2 > this.Count - 1)
			{
				return null;
			}
			Bar bar = this.items[index1];
			for (int i = index1 + 1; i <= index2; i++)
			{
				if (this.items[i].Low < bar.Low)
				{
					bar = this.items[i];
				}
			}
			return bar;
		}
		public Bar LowestLowBar(int nBars)
		{
			return this.LowestLowBar(this.Count - nBars, this.Count - 1);
		}
		public Bar LowestLowBar(DateTime dateTime1, DateTime dateTime2)
		{
			int index = this.GetIndex(dateTime1, IndexOption.Next);
			int index2 = this.GetIndex(dateTime2, IndexOption.Prev);
			return this.LowestLowBar(index, index2);
		}
		public Bar LowestLowBar()
		{
			return this.min;
		}
		public double LowestLow(int index1, int index2)
		{
			return this.LowestLowBar(index1, index2).Low;
		}
		public double LowestLow(int nBars)
		{
			return this.LowestLowBar(nBars).Low;
		}
		public double LowestLow(DateTime dateTime1, DateTime dateTime2)
		{
			return this.LowestLowBar(dateTime1, dateTime2).Low;
		}
		public double LowestLow()
		{
			return this.min.low;
		}
		public Bar LowestHighBar(int index1, int index2)
		{
			if (this.Count == 0)
			{
				return null;
			}
			if (index1 > index2)
			{
				return null;
			}
			if (index1 < 0 || index1 > this.Count - 1)
			{
				return null;
			}
			if (index2 < 0 || index2 > this.Count - 1)
			{
				return null;
			}
			Bar bar = this.items[index1];
			for (int i = index1 + 1; i <= index2; i++)
			{
				if (this.items[i].High < bar.High)
				{
					bar = this.items[i];
				}
			}
			return bar;
		}
		public Bar LowestHighBar(int nBars)
		{
			return this.LowestHighBar(this.Count - nBars, this.Count - 1);
		}
		public Bar LowestHighBar(DateTime dateTime1, DateTime dateTime2)
		{
			int index = this.GetIndex(dateTime1, IndexOption.Next);
			int index2 = this.GetIndex(dateTime2, IndexOption.Prev);
			return this.LowestHighBar(index, index2);
		}
		public double LowestHigh(int index1, int index2)
		{
			return this.LowestHighBar(index1, index2).High;
		}
		public double LowestHigh(int nBars)
		{
			return this.LowestHighBar(nBars).High;
		}
		public double LowestHigh(DateTime dateTime1, DateTime dateTime2)
		{
			return this.LowestHighBar(dateTime1, dateTime2).High;
		}
		public int GetIndex(DateTime datetime, IndexOption option = IndexOption.Null)
		{
			int num = 0;
			int num2 = 0;
			int num3 = this.items.Count - 1;
			bool flag = true;
			while (flag)
			{
				if (num3 < num2)
				{
					return -1;
				}
				num = (num2 + num3) / 2;
				switch (option)
				{
				case IndexOption.Null:
					if (this.items[num].dateTime == datetime)
					{
						flag = false;
					}
					else
					{
						if (this.items[num].dateTime > datetime)
						{
							num3 = num - 1;
						}
						else
						{
							if (this.items[num].dateTime < datetime)
							{
								num2 = num + 1;
							}
						}
					}
					break;
				case IndexOption.Next:
					if (this.items[num].dateTime >= datetime && (num == 0 || this.items[num - 1].dateTime < datetime))
					{
						flag = false;
					}
					else
					{
						if (this.items[num].dateTime < datetime)
						{
							num2 = num + 1;
						}
						else
						{
							num3 = num - 1;
						}
					}
					break;
				case IndexOption.Prev:
					if (this.items[num].dateTime <= datetime && (num == this.items.Count - 1 || this.items[num + 1].dateTime > datetime))
					{
						flag = false;
					}
					else
					{
						if (this.items[num].dateTime > datetime)
						{
							num3 = num - 1;
						}
						else
						{
							num2 = num + 1;
						}
					}
					break;
				}
			}
			return num;
		}
		public DateTime GetDateTime(int index)
		{
			return this.items[index].dateTime;
		}
	}
}
