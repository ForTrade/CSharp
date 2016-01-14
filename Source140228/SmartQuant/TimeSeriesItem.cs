using System;
namespace SmartQuant
{
	public class TimeSeriesItem : DataObject
	{
		internal double value;
		public override byte TypeId
		{
			get
			{
				return 11;
			}
		}
		public double Value
		{
			get
			{
				return this.value;
			}
		}
		public TimeSeriesItem(DateTime dateTime, double value)
		{
			this.dateTime = dateTime;
			this.value = value;
		}
		public TimeSeriesItem(TimeSeriesItem item)
		{
			this.dateTime = item.dateTime;
			this.value = item.value;
		}
		public TimeSeriesItem()
		{
		}
	}
}
