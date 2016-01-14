using System;
using System.Collections;
using System.Collections.Generic;
namespace SmartQuant
{
	public class TimeSeries : ISeries
	{
		private List<TimeSeriesItem> items;
		private TimeSeriesItem min;
		private TimeSeriesItem max;
		private bool changed;
		private double mean;
		private double variance;
		private double sum;
		private double median;
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
		public virtual int Count
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
		public virtual double First
		{
			get
			{
				return this.items[0].value;
			}
		}
		public virtual double Last
		{
			get
			{
				return this.items[this.items.Count - 1].value;
			}
		}
		public virtual DateTime FirstDateTime
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
		public virtual DateTime LastDateTime
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
		public virtual double this[int index]
		{
			get
			{
				return this.items[index].value;
			}
			set
			{
				this.items[index].value = value;
			}
		}
		public virtual double this[int index, BarData barData]
		{
			get
			{
				return this[index];
			}
		}
		public double this[int index, int row]
		{
			get
			{
				return this.items[index].value;
			}
		}
		public double this[DateTime dateTime, SearchOption option = SearchOption.Exact]
		{
			get
			{
				return this.GetByDateTime(dateTime, SearchOption.Exact).value;
			}
			set
			{
				this.Add(dateTime, value);
			}
		}
		public double this[DateTime dateTime, int row]
		{
			get
			{
				return this.GetByDateTime(dateTime, SearchOption.Exact).value;
			}
		}
		public TimeSeries()
		{
			this.items = new List<TimeSeriesItem>();
			this.min = null;
			this.max = null;
			this.changed = true;
			this.indicators = new List<Indicator>();
		}
		public TimeSeries(string name, string description = "")
		{
			this.name = name;
			this.description = description;
			this.items = new List<TimeSeriesItem>();
			this.min = null;
			this.max = null;
			this.changed = true;
			this.indicators = new List<Indicator>();
		}
		public void Clear()
		{
			this.items.Clear();
			this.min = null;
			this.max = null;
			this.changed = true;
		}
		public void Add(DateTime dateTime, double value)
		{
			TimeSeriesItem timeSeriesItem = new TimeSeriesItem(dateTime, value);
			if (this.min == null)
			{
				this.min = timeSeriesItem;
			}
			else
			{
				if (timeSeriesItem.value < this.min.value)
				{
					this.min = timeSeriesItem;
				}
			}
			if (this.max == null)
			{
				this.max = timeSeriesItem;
			}
			else
			{
				if (timeSeriesItem.value > this.max.value)
				{
					this.max = timeSeriesItem;
				}
			}
			this.items.Add(timeSeriesItem);
			int index = this.items.Count - 1;
			for (int i = 0; i < this.indicators.Count; i++)
			{
				this.indicators[i].Calculate(index);
			}
		}
		public void Remove(int index)
		{
			this.items.RemoveAt(index);
		}
		public bool Contains(DateTime dateTime)
		{
			return this.GetByDateTime(dateTime, SearchOption.Exact) != null;
		}
		public TimeSeriesItem GetItem(int index)
		{
			return this.items[index];
		}
		public TimeSeriesItem GetMinItem()
		{
			return this.min;
		}
		public TimeSeriesItem GetMaxItem()
		{
			return this.max;
		}
		public double GetMin()
		{
			if (this.min == null)
			{
				return double.NaN;
			}
			return this.min.value;
		}
		public double GetMax()
		{
			if (this.max == null)
			{
				return double.NaN;
			}
			return this.max.value;
		}
		public double GetValue(int index)
		{
			return this.items[index].value;
		}
		public virtual DateTime GetDateTime(int index)
		{
			return this.items[index].dateTime;
		}
		public TimeSeriesItem GetByDateTime(DateTime dateTime, SearchOption option = SearchOption.Exact)
		{
			int num = this.IndexOf(dateTime, option);
			if (num != -1)
			{
				return this.items[num];
			}
			return null;
		}
		public Cross Crosses(double level, int index)
		{
			if (index <= 0 || index > this.items.Count - 1)
			{
				return Cross.None;
			}
			if (this.items[index - 1].Value <= level && this.items[index].value > level)
			{
				return Cross.Above;
			}
			if (this.items[index - 1].value >= level && this.items[index].value < level)
			{
				return Cross.Below;
			}
			return Cross.None;
		}
		public int IndexOf(DateTime dateTime, SearchOption option = SearchOption.Exact)
		{
			int num = this.items.Count - 1;
			if (dateTime == this.GetDateTime(num))
			{
				return num;
			}
			num = 0;
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
				case SearchOption.Next:
					if (this.items[num].dateTime >= dateTime && (num == 0 || this.items[num - 1].dateTime < dateTime))
					{
						flag = false;
					}
					else
					{
						if (this.items[num].dateTime < dateTime)
						{
							num2 = num + 1;
						}
						else
						{
							num3 = num - 1;
						}
					}
					break;
				case SearchOption.Prev:
					if (this.items[num].dateTime <= dateTime && (num == this.items.Count - 1 || this.items[num + 1].dateTime > dateTime))
					{
						flag = false;
					}
					else
					{
						if (this.items[num].dateTime > dateTime)
						{
							num3 = num - 1;
						}
						else
						{
							num2 = num + 1;
						}
					}
					break;
				case SearchOption.Exact:
					if (this.items[num].dateTime == dateTime)
					{
						flag = false;
					}
					else
					{
						if (this.items[num].dateTime > dateTime)
						{
							num3 = num - 1;
						}
						else
						{
							if (this.items[num].dateTime < dateTime)
							{
								num2 = num + 1;
							}
						}
					}
					break;
				}
			}
			return num;
		}
		public Cross Crosses(TimeSeries series, DateTime dateTime)
		{
			int num = this.IndexOf(dateTime, SearchOption.Exact);
			int num2 = series.IndexOf(dateTime, SearchOption.Exact);
			if (num <= 0 || num >= this.items.Count)
			{
				return Cross.None;
			}
			if (num2 <= 0 || num2 >= series.Count)
			{
				return Cross.None;
			}
			DateTime dateTime2 = this.GetDateTime(num - 1);
			DateTime dateTime3 = series.GetDateTime(num2 - 1);
			if (dateTime2 == dateTime3)
			{
				if (this.GetValue(num - 1) <= series.GetValue(num2 - 1) && this.GetValue(num) > series.GetValue(num2))
				{
					return Cross.Above;
				}
				if (this.GetValue(num - 1) >= series.GetValue(num2 - 1) && this.GetValue(num) < series.GetValue(num2))
				{
					return Cross.Below;
				}
			}
			else
			{
				double value;
				double value2;
				if (dateTime2 < dateTime3)
				{
					DateTime dateTime4 = this.GetDateTime(num - 1);
					value = this.GetValue(num - 1);
					if (series.IndexOf(dateTime4, SearchOption.Next) != num2)
					{
						value2 = series.GetValue(series.IndexOf(dateTime4, SearchOption.Next));
					}
					else
					{
						value2 = series.GetValue(series.IndexOf(dateTime4, SearchOption.Prev));
					}
				}
				else
				{
					DateTime dateTime5 = series.GetDateTime(num2 - 1);
					value2 = series.GetValue(num2 - 1);
					if (this.IndexOf(dateTime5, SearchOption.Prev) != num)
					{
						value = this.GetValue(this.IndexOf(dateTime5, SearchOption.Next));
					}
					else
					{
						value = this.GetValue(this.IndexOf(dateTime5, SearchOption.Prev));
					}
				}
				if (value <= value2 && this.GetValue(num) > series.GetValue(num2))
				{
					return Cross.Above;
				}
				if (value >= value2 && this.GetValue(num) < series.GetValue(num2))
				{
					return Cross.Below;
				}
			}
			return Cross.None;
		}
		public virtual double GetMin(DateTime dateTime1, DateTime dateTime2)
		{
			TimeSeriesItem timeSeriesItem = null;
			for (int i = 0; i < this.items.Count; i++)
			{
				TimeSeriesItem timeSeriesItem2 = this.items[i];
				if (!(timeSeriesItem2.DateTime < dateTime1))
				{
					if (timeSeriesItem2.DateTime > dateTime2)
					{
						if (timeSeriesItem != null)
						{
							return timeSeriesItem.value;
						}
						return double.NaN;
					}
					else
					{
						if (timeSeriesItem == null)
						{
							timeSeriesItem = timeSeriesItem2;
						}
						else
						{
							if (timeSeriesItem2.value < timeSeriesItem.value)
							{
								timeSeriesItem = timeSeriesItem2;
							}
						}
					}
				}
			}
			if (timeSeriesItem != null)
			{
				return timeSeriesItem.value;
			}
			return double.NaN;
		}
		public virtual double GetMin(int index1, int index2, BarData barData)
		{
			return this.GetMin(index1, index2);
		}
		public double GetMin(int index1, int index2)
		{
			TimeSeriesItem timeSeriesItem = null;
			for (int i = index1; i <= index2; i++)
			{
				TimeSeriesItem timeSeriesItem2 = this.items[i];
				if (timeSeriesItem == null)
				{
					timeSeriesItem = timeSeriesItem2;
				}
				else
				{
					if (timeSeriesItem2.value < timeSeriesItem.value)
					{
						timeSeriesItem = timeSeriesItem2;
					}
				}
			}
			if (timeSeriesItem != null)
			{
				return timeSeriesItem.value;
			}
			return double.NaN;
		}
		public virtual double GetMax(DateTime dateTime1, DateTime dateTime2)
		{
			TimeSeriesItem timeSeriesItem = null;
			for (int i = 0; i < this.items.Count; i++)
			{
				TimeSeriesItem timeSeriesItem2 = this.items[i];
				if (!(timeSeriesItem2.DateTime < dateTime1))
				{
					if (timeSeriesItem2.DateTime > dateTime2)
					{
						if (timeSeriesItem != null)
						{
							return timeSeriesItem.value;
						}
						return double.NaN;
					}
					else
					{
						if (timeSeriesItem == null)
						{
							timeSeriesItem = timeSeriesItem2;
						}
						else
						{
							if (timeSeriesItem2.value > timeSeriesItem.value)
							{
								timeSeriesItem = timeSeriesItem2;
							}
						}
					}
				}
			}
			if (timeSeriesItem != null)
			{
				return timeSeriesItem.value;
			}
			return double.NaN;
		}
		public virtual double GetMax(int index1, int index2, BarData barData)
		{
			return this.GetMax(index1, index2);
		}
		public double GetMax(int index1, int index2)
		{
			TimeSeriesItem timeSeriesItem = null;
			for (int i = index1; i <= index2; i++)
			{
				TimeSeriesItem timeSeriesItem2 = this.items[i];
				if (timeSeriesItem == null)
				{
					timeSeriesItem = timeSeriesItem2;
				}
				else
				{
					if (timeSeriesItem2.value > timeSeriesItem.value)
					{
						timeSeriesItem = timeSeriesItem2;
					}
				}
			}
			if (timeSeriesItem != null)
			{
				return timeSeriesItem.value;
			}
			return double.NaN;
		}
		public virtual int GetIndex(DateTime datetime, IndexOption option = IndexOption.Null)
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
		public double GetSum()
		{
			if (this.changed)
			{
				this.sum = 0.0;
				for (int i = 0; i < this.Count; i++)
				{
					this.sum += this[i, 0];
				}
			}
			return this.sum;
		}
		public double GetSum(int index1, int index2, int row)
		{
			if (index1 >= index2)
			{
				throw new ApplicationException("index1 must be smaller than index2");
			}
			if (index1 < 0 || index1 > this.Count - 1)
			{
				throw new ApplicationException("index1 is out of range");
			}
			if (index2 < 0 || index2 > this.Count - 1)
			{
				throw new ApplicationException("index2 is out of range");
			}
			double num = 0.0;
			for (int i = index1; i <= index2; i++)
			{
				num += this[i, row];
			}
			return num;
		}
		public double GetMean()
		{
			if (this.Count <= 0)
			{
				throw new ApplicationException("Can not calculate mean. Array is empty.");
			}
			if (this.changed)
			{
				this.mean = this.GetMean(0, this.Count - 1);
			}
			return this.mean;
		}
		public virtual double GetMean(int index1, int index2)
		{
			return this.GetMean(index1, index2, 0);
		}
		public virtual double GetMean(DateTime dateTime1, DateTime dateTime2)
		{
			return this.GetMean(dateTime1, dateTime2, 0);
		}
		public virtual double GetMean(int row)
		{
			return this.GetMean(0, this.Count - 1, row);
		}
		public double GetMean(int index1, int index2, int row)
		{
			if (this.Count <= 0)
			{
				throw new ApplicationException("Can not calculate mean. Array is empty.");
			}
			if (index1 > index2)
			{
				throw new ApplicationException("index1 must be smaller than index2");
			}
			if (index1 < 0 || index1 > this.Count - 1)
			{
				throw new ApplicationException("index1 is out of range");
			}
			if (index2 < 0 || index2 > this.Count - 1)
			{
				throw new ApplicationException("index2 is out of range");
			}
			double num = 0.0;
			for (int i = index1; i <= index2; i++)
			{
				num += this[i, row];
			}
			return num / (double)(index2 - index1 + 1);
		}
		public double GetMean(DateTime dateTime1, DateTime dateTime2, int row)
		{
			if (this.Count <= 0)
			{
				throw new ApplicationException("Can not calculate mean. Array is empty.");
			}
			if (dateTime1 >= dateTime2)
			{
				throw new ApplicationException("dateTime1 must be smaller than dateTime2");
			}
			int index = this.GetIndex(dateTime1, IndexOption.Null);
			int index2 = this.GetIndex(dateTime2, IndexOption.Null);
			if (index == -1)
			{
				throw new ApplicationException("dateTime1 is out of range");
			}
			if (index2 == -1)
			{
				throw new ApplicationException("dateTime2 is out of range");
			}
			return this.GetMean(index, index2, row);
		}
		public virtual double GetMedian(int index1, int index2)
		{
			return this.GetMedian(index1, index2, 0);
		}
		public virtual double GetMedian(DateTime dateTime1, DateTime dateTime2)
		{
			return this.GetMedian(dateTime1, dateTime2, 0);
		}
		public virtual double GetMedian(int row)
		{
			return this.GetMedian(0, this.Count - 1, row);
		}
		public double GetMedian()
		{
			if (this.Count <= 0)
			{
				throw new ApplicationException("Can not calculate median. Array is empty.");
			}
			if (this.changed)
			{
				this.median = this.GetMedian(0, this.Count - 1);
			}
			return this.median;
		}
		public double GetMedian(int index1, int index2, int row)
		{
			if (this.Count <= 0)
			{
				throw new ApplicationException("Can not calculate mean. Array is empty.");
			}
			if (index1 > index2)
			{
				throw new ApplicationException("index1 must be smaller than index2");
			}
			if (index1 < 0 || index1 > this.Count - 1)
			{
				throw new ApplicationException("index1 is out of range");
			}
			if (index2 < 0 || index2 > this.Count - 1)
			{
				throw new ApplicationException("index2 is out of range");
			}
			ArrayList arrayList = new ArrayList();
			for (int i = index1; i <= index2; i++)
			{
				arrayList.Add(this[i, row]);
			}
			arrayList.Sort();
			return (double)arrayList[arrayList.Count / 2];
		}
		public double GetMedian(DateTime dateTime1, DateTime dateTime2, int row)
		{
			if (this.Count <= 0)
			{
				throw new ApplicationException("Can not calculate mean. Array is empty.");
			}
			if (dateTime1 >= dateTime2)
			{
				throw new ApplicationException("dateTime1 must be smaller than dateTime2");
			}
			int index = this.GetIndex(dateTime1, IndexOption.Null);
			int index2 = this.GetIndex(dateTime2, IndexOption.Null);
			if (index == -1)
			{
				throw new ApplicationException("dateTime1 is out of range");
			}
			if (index2 == -1)
			{
				throw new ApplicationException("dateTime2 is out of range");
			}
			return this.GetMedian(index, index2, row);
		}
		public double GetVariance()
		{
			if (this.Count <= 1)
			{
				throw new ApplicationException("Can not calculate variance. Insufficient number of elements in the array.");
			}
			if (this.changed)
			{
				double num = this.GetMean();
				this.variance = 0.0;
				for (int i = 0; i < this.Count; i++)
				{
					this.variance += (num - this[i, 0]) * (num - this[i, 0]);
				}
				this.variance /= (double)(this.Count - 1);
			}
			return this.variance;
		}
		public virtual double GetVariance(int index1, int index2)
		{
			return this.GetVariance(index1, index2, 0);
		}
		public virtual double GetVariance(DateTime dateTime1, DateTime dateTime2)
		{
			return this.GetVariance(dateTime1, dateTime2, 0);
		}
		public virtual double GetVariance(int row)
		{
			return this.GetVariance(0, this.Count - 1, row);
		}
		public double GetVariance(int index1, int index2, int row)
		{
			if (this.Count <= 1)
			{
				throw new ApplicationException("Can not calculate variance. Insufficient number of elements in the array.");
			}
			if (index1 > index2)
			{
				throw new ApplicationException("index1 must be smaller than index2");
			}
			if (index1 < 0 || index1 > this.Count - 1)
			{
				throw new ApplicationException("index1 is out of range");
			}
			if (index2 < 0 || index2 > this.Count - 1)
			{
				throw new ApplicationException("index2 is out of range");
			}
			double num = this.GetMean(index1, index2, row);
			double num2 = 0.0;
			for (int i = index1; i <= index2; i++)
			{
				num2 += (num - this[i, row]) * (num - this[i, row]);
			}
			return num2 / (double)(index2 - index1);
		}
		public virtual double GetVariance(DateTime dateTime1, DateTime dateTime2, int row)
		{
			if (this.Count <= 1)
			{
				throw new ApplicationException("Can not calculate variance. Insufficient number of elements in the array.");
			}
			if (dateTime1 >= dateTime2)
			{
				throw new ApplicationException("dateTime1 must be smaller than dateTime2");
			}
			int index = this.GetIndex(dateTime1, IndexOption.Null);
			int index2 = this.GetIndex(dateTime2, IndexOption.Null);
			if (index == -1)
			{
				throw new ApplicationException("dateTime1 is out of range");
			}
			if (index2 == -1)
			{
				throw new ApplicationException("dateTime2 is out of range");
			}
			return this.GetVariance(index, index2, row);
		}
		public virtual double GetPositiveVariance()
		{
			return this.GetPositiveVariance(0);
		}
		public double GetPositiveVariance(int index1, int index2, int row)
		{
			if (this.Count <= 1)
			{
				throw new ApplicationException("Can not calculate variance. Insufficient number of elements in the array.");
			}
			if (index1 > index2)
			{
				throw new ApplicationException("index1 must be smaller than index2");
			}
			if (index1 < 0 || index1 > this.Count - 1)
			{
				throw new ApplicationException("index1 is out of range");
			}
			if (index2 < 0 || index2 > this.Count - 1)
			{
				throw new ApplicationException("index2 is out of range");
			}
			int num = 0;
			double num2 = 0.0;
			for (int i = index1; i <= index2; i++)
			{
				if (this[i, row] > 0.0)
				{
					num2 += this[i, row];
					num++;
				}
			}
			num2 /= (double)num;
			double num3 = 0.0;
			for (int j = index1; j <= index2; j++)
			{
				if (this[j, row] > 0.0)
				{
					num3 += (num2 - this[j, row]) * (num2 - this[j, row]);
				}
			}
			return num3 / (double)num;
		}
		public virtual double GetPositiveVariance(DateTime dateTime1, DateTime dateTime2, int row)
		{
			if (this.Count <= 1)
			{
				throw new ApplicationException("Can not calculate variance. Insufficient number of elements in the array.");
			}
			if (dateTime1 >= dateTime2)
			{
				throw new ApplicationException("dateTime1 must be smaller than dateTime2");
			}
			int index = this.GetIndex(dateTime1, IndexOption.Null);
			int index2 = this.GetIndex(dateTime2, IndexOption.Null);
			if (index == -1)
			{
				throw new ApplicationException("dateTime1 is out of range");
			}
			if (index2 == -1)
			{
				throw new ApplicationException("dateTime2 is out of range");
			}
			return this.GetPositiveVariance(index, index2, row);
		}
		public virtual double GetPositiveVariance(int index1, int index2)
		{
			return this.GetPositiveVariance(index1, index2, 0);
		}
		public virtual double GetPositiveVariance(DateTime dateTime1, DateTime dateTime2)
		{
			return this.GetPositiveVariance(dateTime1, dateTime2, 0);
		}
		public virtual double GetPositiveVariance(int row)
		{
			return this.GetPositiveVariance(0, this.Count - 1, row);
		}
		public virtual double GetNegativeVariance()
		{
			return this.GetNegativeVariance(0);
		}
		public virtual double GetNegativeVariance(int index1, int index2)
		{
			return this.GetNegativeVariance(index1, index2, 0);
		}
		public virtual double GetNegativeVariance(DateTime dateTime1, DateTime dateTime2)
		{
			return this.GetNegativeVariance(dateTime1, dateTime2, 0);
		}
		public virtual double GetNegativeVariance(int row)
		{
			return this.GetNegativeVariance(0, this.Count - 1, row);
		}
		public double GetNegativeVariance(int index1, int index2, int row)
		{
			if (this.Count <= 1)
			{
				throw new ApplicationException("Can not calculate variance. Insufficient number of elements in the array.");
			}
			if (index1 > index2)
			{
				throw new ApplicationException("index1 must be smaller than index2");
			}
			if (index1 < 0 || index1 > this.Count - 1)
			{
				throw new ApplicationException("index1 is out of range");
			}
			if (index2 < 0 || index2 > this.Count - 1)
			{
				throw new ApplicationException("index2 is out of range");
			}
			int num = 0;
			double num2 = 0.0;
			for (int i = index1; i <= index2; i++)
			{
				if (this[i, row] < 0.0)
				{
					num2 += this[i, row];
					num++;
				}
			}
			num2 /= (double)num;
			double num3 = 0.0;
			for (int j = index1; j <= index2; j++)
			{
				if (this[j, row] < 0.0)
				{
					num3 += (num2 - this[j, row]) * (num2 - this[j, row]);
				}
			}
			return num3 / (double)num;
		}
		public virtual double GetNegativeVariance(DateTime dateTime1, DateTime dateTime2, int row)
		{
			if (this.Count <= 1)
			{
				throw new ApplicationException("Can not calculate variance. Insufficient number of elements in the array.");
			}
			if (dateTime1 >= dateTime2)
			{
				throw new ApplicationException("dateTime1 must be smaller than dateTime2");
			}
			int index = this.GetIndex(dateTime1, IndexOption.Null);
			int index2 = this.GetIndex(dateTime2, IndexOption.Null);
			if (index == -1)
			{
				throw new ApplicationException("dateTime1 is out of range");
			}
			if (index2 == -1)
			{
				throw new ApplicationException("dateTime2 is out of range");
			}
			return this.GetNegativeVariance(index, index2, row);
		}
		public double GetStdDev()
		{
			return Math.Sqrt(this.GetVariance());
		}
		public double GetStdDev(int index1, int index2)
		{
			return Math.Sqrt(this.GetVariance(index1, index2));
		}
		public double GetStdDev(DateTime dateTime1, DateTime dateTime2)
		{
			return Math.Sqrt(this.GetVariance(dateTime1, dateTime2));
		}
		public double GetStdDev(int row)
		{
			return Math.Sqrt(this.GetVariance(row));
		}
		public double GetStdDev(int index1, int index2, int row)
		{
			return Math.Sqrt(this.GetVariance(index1, index2, row));
		}
		public double GetStdDev(DateTime dateTime1, DateTime dateTime2, int row)
		{
			return Math.Sqrt(this.GetVariance(dateTime1, dateTime2, row));
		}
		public double GetPositiveStdDev()
		{
			return Math.Sqrt(this.GetPositiveVariance());
		}
		public double GetPositiveStdDev(int index1, int index2)
		{
			return Math.Sqrt(this.GetPositiveVariance(index1, index2));
		}
		public double GetPositiveStdDev(DateTime dateTime1, DateTime dateTime2)
		{
			return Math.Sqrt(this.GetPositiveVariance(dateTime1, dateTime2));
		}
		public double GetPositiveStdDev(int row)
		{
			return Math.Sqrt(this.GetPositiveVariance(row));
		}
		public double GetPositiveStdDev(int index1, int index2, int row)
		{
			return Math.Sqrt(this.GetPositiveVariance(index1, index2, row));
		}
		public double GetPositiveStdDev(DateTime dateTime1, DateTime dateTime2, int row)
		{
			return Math.Sqrt(this.GetPositiveVariance(dateTime1, dateTime2, row));
		}
		public double GetNegativeStdDev()
		{
			return Math.Sqrt(this.GetNegativeVariance());
		}
		public double GetNegativeStdDev(int index1, int index2)
		{
			return Math.Sqrt(this.GetNegativeVariance(index1, index2));
		}
		public double GetNegativeStdDev(DateTime dateTime1, DateTime dateTime2)
		{
			return Math.Sqrt(this.GetNegativeVariance(dateTime1, dateTime2));
		}
		public double GetNegativeStdDev(int row)
		{
			return Math.Sqrt(this.GetNegativeVariance(row));
		}
		public double GetNegativeStdDev(int index1, int index2, int row)
		{
			return Math.Sqrt(this.GetNegativeVariance(index1, index2, row));
		}
		public double GetNegativeStdDev(DateTime dateTime1, DateTime dateTime2, int row)
		{
			return Math.Sqrt(this.GetNegativeVariance(dateTime1, dateTime2, row));
		}
		public double GetMoment(int k, int index1, int index2, int row)
		{
			if (this.Count <= 0)
			{
				throw new ApplicationException("Can not calculate momentum. Series " + this.name + " is empty.");
			}
			if (index1 > index2)
			{
				throw new ApplicationException("index1 must be smaller than index2");
			}
			if (index1 < 0 || index1 > this.Count - 1)
			{
				throw new ApplicationException("index1 is out of range");
			}
			if (index2 < 0 || index2 > this.Count - 1)
			{
				throw new ApplicationException("index2 is out of range");
			}
			double num;
			if (k == 1)
			{
				num = 0.0;
			}
			else
			{
				num = this.GetMean(index1, index2, row);
			}
			int num2 = 0;
			double num3 = 0.0;
			for (int i = index1; i <= index2; i++)
			{
				num3 += Math.Pow(this[i, row] - num, (double)k);
				num2++;
			}
			if (num2 == 0)
			{
				return 0.0;
			}
			return num3 / (double)num2;
		}
		public double GetAsymmetry(int index1, int index2, int row)
		{
			if (this.Count <= 0)
			{
				throw new ApplicationException("Can not calculate asymmetry. Series " + this.name + " is empty.");
			}
			if (index1 > index2)
			{
				throw new ApplicationException("index1 must be smaller than index2");
			}
			if (index1 < 0 || index1 > this.Count - 1)
			{
				throw new ApplicationException("index1 is out of range");
			}
			if (index2 < 0 || index2 > this.Count - 1)
			{
				throw new ApplicationException("index2 is out of range");
			}
			double stdDev = this.GetStdDev(index1, index2, row);
			if (stdDev == 0.0)
			{
				return 0.0;
			}
			return this.GetMoment(3, index1, index2, row) / Math.Pow(stdDev, 3.0);
		}
		public double GetExcess(int index1, int index2, int row)
		{
			if (this.Count <= 0)
			{
				throw new ApplicationException("Can not calculate excess. Series " + this.name + " is empty.");
			}
			if (index1 > index2)
			{
				throw new ApplicationException("index1 must be smaller than index2");
			}
			if (index1 < 0 || index1 > this.Count - 1)
			{
				throw new ApplicationException("index1 is out of range");
			}
			if (index2 < 0 || index2 > this.Count - 1)
			{
				throw new ApplicationException("index2 is out of range");
			}
			double stdDev = this.GetStdDev(index1, index2, row);
			if (stdDev == 0.0)
			{
				return 0.0;
			}
			return this.GetMoment(4, index1, index2, row) / Math.Pow(stdDev, 4.0);
		}
		public double GetCovariance(int row1, int row2, int index1, int index2)
		{
			if (this.Count <= 0)
			{
				throw new ApplicationException("Can not calculate covariance. Array is empty.");
			}
			if (index1 > index2)
			{
				throw new ApplicationException("index1 must be smaller than index2");
			}
			if (index1 < 0 || index1 > this.Count - 1)
			{
				throw new ApplicationException("index1 is out of range");
			}
			if (index2 < 0 || index2 > this.Count - 1)
			{
				throw new ApplicationException("index2 is out of range");
			}
			double num = this.GetMean(index1, index2, row1);
			double num2 = this.GetMean(index1, index2, row2);
			double num3 = 0.0;
			double num4 = 0.0;
			for (int i = index1; i <= index2; i++)
			{
				num4 += (this[i, row1] - num) * (this[i, row2] - num2);
				num3 += 1.0;
			}
			if (num3 <= 1.0)
			{
				return 0.0;
			}
			return num4 / (num3 - 1.0);
		}
		public double GetCorrelation(int row1, int row2, int index1, int index2)
		{
			return this.GetCovariance(row1, row2, index1, index2) / (this.GetStdDev(index1, index2, row1) * this.GetStdDev(index1, index2, row2));
		}
		public double GetCovariance(TimeSeries series)
		{
			if (series == null)
			{
				throw new ArgumentException("Argument series should be of TimeSeries type");
			}
			double num = this.GetMean();
			double num2 = series.GetMean();
			double num3 = 0.0;
			double num4 = 0.0;
			for (int i = 0; i < this.Count; i++)
			{
				DateTime dateTime = this.GetDateTime(i);
				if (series.Contains(dateTime))
				{
					num4 += (this[i] - num) * (series[dateTime, SearchOption.Exact] - num2);
					num3 += 1.0;
				}
			}
			if (num3 <= 1.0)
			{
				return 0.0;
			}
			return num4 / (num3 - 1.0);
		}
		public double GetCorrelation(TimeSeries series)
		{
			return this.GetCovariance(series) / (this.GetStdDev() * series.GetStdDev());
		}
		public virtual TimeSeries Log()
		{
			TimeSeries timeSeries = base.GetType().GetConstructor(new Type[0]).Invoke(new object[0]) as TimeSeries;
			timeSeries.name = "Log(" + this.name + ")";
			timeSeries.description = this.description;
			for (int i = 0; i < this.Count; i++)
			{
				timeSeries.Add(this.GetDateTime(i), Math.Log(this[i, 0]));
			}
			return timeSeries;
		}
		public TimeSeries Log10()
		{
			TimeSeries timeSeries = base.GetType().GetConstructor(new Type[0]).Invoke(new object[0]) as TimeSeries;
			timeSeries.name = "Log10(" + this.name + ")";
			timeSeries.description = this.description;
			for (int i = 0; i < this.Count; i++)
			{
				timeSeries.Add(this.GetDateTime(i), Math.Log10(this[i, 0]));
			}
			return timeSeries;
		}
		public TimeSeries Sqrt()
		{
			TimeSeries timeSeries = base.GetType().GetConstructor(new Type[0]).Invoke(new object[0]) as TimeSeries;
			timeSeries.name = "Sqrt(" + this.name + ")";
			timeSeries.description = this.description;
			for (int i = 0; i < this.Count; i++)
			{
				timeSeries.Add(this.GetDateTime(i), Math.Sqrt(this[i, 0]));
			}
			return timeSeries;
		}
		public TimeSeries Exp()
		{
			TimeSeries timeSeries = base.GetType().GetConstructor(new Type[0]).Invoke(new object[0]) as TimeSeries;
			timeSeries.name = "Exp(" + this.name + ")";
			timeSeries.description = this.description;
			for (int i = 0; i < this.Count; i++)
			{
				timeSeries.Add(this.GetDateTime(i), Math.Exp(this[i, 0]));
			}
			return timeSeries;
		}
		public TimeSeries Pow(double Pow)
		{
			TimeSeries timeSeries = base.GetType().GetConstructor(new Type[0]).Invoke(new object[0]) as TimeSeries;
			timeSeries.name = "Pow(" + this.name + ")";
			timeSeries.description = this.description;
			for (int i = 0; i < this.Count; i++)
			{
				timeSeries.Add(this.GetDateTime(i), Math.Pow(this[i, 0], Pow));
			}
			return timeSeries;
		}
		public virtual double GetAutoCovariance(int Lag)
		{
			if (Lag >= this.Count)
			{
				throw new ApplicationException("Not enough data points in the series to calculate autocovariance");
			}
			double num = this.GetMean();
			double num2 = 0.0;
			for (int i = Lag; i < this.Count; i++)
			{
				num2 += (this[i, 0] - num) * (this[i - Lag, 0] - num);
			}
			return num2 / (double)(this.Count - Lag);
		}
		public double GetAutoCorrelation(int Lag)
		{
			return this.GetAutoCovariance(Lag) / this.GetVariance();
		}
		public virtual TimeSeries GetReturnSeries()
		{
			TimeSeries timeSeries = new TimeSeries(this.name, this.description + " (return)");
			if (this.Count > 1)
			{
				double num = this[0];
				for (int i = 0; i < this.Count; i++)
				{
					DateTime dateTime = this.GetDateTime(i);
					double num2 = this[i];
					if (num != 0.0)
					{
						timeSeries.Add(dateTime, num2 / num);
					}
					else
					{
						timeSeries.Add(dateTime, 0.0);
					}
					num = num2;
				}
			}
			return timeSeries;
		}
		public virtual TimeSeries GetPercentReturnSeries()
		{
			TimeSeries timeSeries = new TimeSeries(this.name, this.description + " (% return)");
			if (this.Count > 1)
			{
				double num = this[0];
				for (int i = 0; i < this.Count; i++)
				{
					DateTime dateTime = this.GetDateTime(i);
					double num2 = this[i];
					if (num != 0.0)
					{
						timeSeries.Add(dateTime, (num2 / num - 1.0) * 100.0);
					}
					else
					{
						timeSeries.Add(dateTime, 0.0);
					}
					num = num2;
				}
			}
			return timeSeries;
		}
		public virtual TimeSeries GetPositiveSeries()
		{
			TimeSeries timeSeries = new TimeSeries();
			for (int i = 0; i < this.Count; i++)
			{
				if (this[i] > 0.0)
				{
					timeSeries.Add(this.GetDateTime(i), this[i]);
				}
			}
			return timeSeries;
		}
		public virtual TimeSeries GetNegativeSeries()
		{
			TimeSeries timeSeries = new TimeSeries();
			for (int i = 0; i < this.Count; i++)
			{
				if (this[i] < 0.0)
				{
					timeSeries.Add(this.GetDateTime(i), this[i]);
				}
			}
			return timeSeries;
		}
		public TimeSeries Shift(int offset)
		{
			TimeSeries timeSeries = new TimeSeries(this.name, this.description);
			int num = 0;
			if (offset < 0)
			{
				num += Math.Abs(offset);
			}
			for (int i = num; i < this.Count; i++)
			{
				int num2 = i + offset;
				if (num2 >= this.Count)
				{
					break;
				}
				DateTime dateTime = this.GetDateTime(num2);
				double value = this[i];
				timeSeries[dateTime, SearchOption.Exact] = value;
			}
			return timeSeries;
		}
		public double Ago(int n)
		{
			int num = this.Count - 1 - n;
			if (num < 0)
			{
				throw new ArgumentException("Can not return an entry " + n + " entries ago: time series is too short.");
			}
			return this[num];
		}
		public static TimeSeries operator +(TimeSeries series1, TimeSeries series2)
		{
			if (series1 == null || series2 == null)
			{
				throw new ArgumentException("Operator argument can not be null");
			}
			TimeSeries timeSeries = new TimeSeries(string.Concat(new string[]
			{
				"(",
				series1.Name,
				"+",
				series2.Name,
				")"
			}), "");
			for (int i = 0; i < series1.Count; i++)
			{
				DateTime dateTime = series1.GetDateTime(i);
				if (series2.Contains(dateTime))
				{
					timeSeries.Add(dateTime, series1[dateTime, 0] + series2[dateTime, 0]);
				}
			}
			return timeSeries;
		}
		public static TimeSeries operator -(TimeSeries series1, TimeSeries series2)
		{
			if (series1 == null || series2 == null)
			{
				throw new ArgumentException("Operator argument can not be null");
			}
			TimeSeries timeSeries = new TimeSeries(string.Concat(new string[]
			{
				"(",
				series1.Name,
				"-",
				series2.Name,
				")"
			}), "");
			for (int i = 0; i < series1.Count; i++)
			{
				DateTime dateTime = series1.GetDateTime(i);
				if (series2.Contains(dateTime))
				{
					timeSeries.Add(dateTime, series1[dateTime, 0] - series2[dateTime, 0]);
				}
			}
			return timeSeries;
		}
		public static TimeSeries operator *(TimeSeries series1, TimeSeries series2)
		{
			if (series1 == null || series2 == null)
			{
				throw new ArgumentException("Operator argument can not be null");
			}
			TimeSeries timeSeries = new TimeSeries(string.Concat(new string[]
			{
				"(",
				series1.Name,
				"*",
				series2.Name,
				")"
			}), "");
			for (int i = 0; i < series1.Count; i++)
			{
				DateTime dateTime = series1.GetDateTime(i);
				if (series2.Contains(dateTime))
				{
					timeSeries.Add(dateTime, series1[dateTime, 0] * series2[dateTime, 0]);
				}
			}
			return timeSeries;
		}
		public static TimeSeries operator /(TimeSeries series1, TimeSeries series2)
		{
			if (series1 == null || series2 == null)
			{
				throw new ArgumentException("Operator argument can not be null");
			}
			TimeSeries timeSeries = new TimeSeries(string.Concat(new string[]
			{
				"(",
				series1.Name,
				"/",
				series2.Name,
				")"
			}), "");
			for (int i = 0; i < series1.Count; i++)
			{
				DateTime dateTime = series1.GetDateTime(i);
				if (series2.Contains(dateTime) && series2[dateTime, SearchOption.Exact] != 0.0)
				{
					timeSeries.Add(dateTime, series1[dateTime, 0] / series2[dateTime, 0]);
				}
			}
			return timeSeries;
		}
		public static TimeSeries operator +(TimeSeries series, double Value)
		{
			if (series == null)
			{
				throw new ArgumentException("Operator argument can not be null");
			}
			TimeSeries timeSeries = new TimeSeries(string.Concat(new string[]
			{
				"(",
				series.Name,
				"+",
				Value.ToString("F2"),
				")"
			}), "");
			for (int i = 0; i < series.Count; i++)
			{
				timeSeries.Add(series.GetDateTime(i), series[i, 0] + Value);
			}
			return timeSeries;
		}
		public static TimeSeries operator -(TimeSeries series, double Value)
		{
			if (series == null)
			{
				throw new ArgumentException("Operator argument can not be null");
			}
			TimeSeries timeSeries = new TimeSeries(string.Concat(new string[]
			{
				"(",
				series.Name,
				"-",
				Value.ToString("F2"),
				")"
			}), "");
			for (int i = 0; i < series.Count; i++)
			{
				timeSeries.Add(series.GetDateTime(i), series[i, 0] - Value);
			}
			return timeSeries;
		}
		public static TimeSeries operator *(TimeSeries series, double Value)
		{
			if (series == null)
			{
				throw new ArgumentException("Operator argument can not be null");
			}
			TimeSeries timeSeries = new TimeSeries(string.Concat(new string[]
			{
				"(",
				series.Name,
				"*",
				Value.ToString("F2"),
				")"
			}), "");
			for (int i = 0; i < series.Count; i++)
			{
				timeSeries.Add(series.GetDateTime(i), series[i, 0] * Value);
			}
			return timeSeries;
		}
		public static TimeSeries operator /(TimeSeries series, double Value)
		{
			if (series == null)
			{
				throw new ArgumentException("Operator argument can not be null");
			}
			TimeSeries timeSeries = new TimeSeries(string.Concat(new string[]
			{
				"(",
				series.Name,
				"/",
				Value.ToString("F2"),
				")"
			}), "");
			for (int i = 0; i < series.Count; i++)
			{
				timeSeries.Add(series.GetDateTime(i), series[i, 0] / Value);
			}
			return timeSeries;
		}
		public static TimeSeries operator +(double Value, TimeSeries series)
		{
			if (series == null)
			{
				throw new ArgumentException("Operator argument can not be null");
			}
			TimeSeries timeSeries = new TimeSeries(string.Concat(new string[]
			{
				"(",
				Value.ToString("F2"),
				"+",
				series.Name,
				")"
			}), "");
			for (int i = 0; i < series.Count; i++)
			{
				timeSeries.Add(series.GetDateTime(i), Value + series[i, 0]);
			}
			return timeSeries;
		}
		public static TimeSeries operator -(double Value, TimeSeries series)
		{
			if (series == null)
			{
				throw new ArgumentException("Operator argument can not be null");
			}
			TimeSeries timeSeries = new TimeSeries(string.Concat(new string[]
			{
				"(",
				Value.ToString("F2"),
				"-",
				series.Name,
				")"
			}), "");
			for (int i = 0; i < series.Count; i++)
			{
				timeSeries.Add(series.GetDateTime(i), Value - series[i, 0]);
			}
			return timeSeries;
		}
		public static TimeSeries operator *(double Value, TimeSeries series)
		{
			if (series == null)
			{
				throw new ArgumentException("Operator argument can not be null");
			}
			TimeSeries timeSeries = new TimeSeries(string.Concat(new string[]
			{
				"(",
				Value.ToString("F2"),
				"*",
				series.Name,
				")"
			}), "");
			for (int i = 0; i < series.Count; i++)
			{
				timeSeries.Add(series.GetDateTime(i), Value * series[i, 0]);
			}
			return timeSeries;
		}
		public static TimeSeries operator /(double Value, TimeSeries series)
		{
			if (series == null)
			{
				throw new ArgumentException("Operator argument can not be null");
			}
			TimeSeries timeSeries = new TimeSeries(string.Concat(new string[]
			{
				"(",
				Value.ToString("F2"),
				"/",
				series.Name,
				")"
			}), "");
			for (int i = 0; i < series.Count; i++)
			{
				if (series[i, 0] != 0.0)
				{
					timeSeries.Add(series.GetDateTime(i), Value / series[i, 0]);
				}
			}
			return timeSeries;
		}
	}
}
