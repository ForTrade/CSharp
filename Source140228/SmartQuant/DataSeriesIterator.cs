using System;
namespace SmartQuant
{
	public class DataSeriesIterator
	{
		private DataSeries series;
		private long index1;
		private long index2;
		private long current;
		public DataSeriesIterator(DataSeries series, long index1 = -1L, long index2 = -1L)
		{
			this.series = series;
			if (index1 == -1L)
			{
				this.index1 = 0L;
			}
			else
			{
				this.index1 = index1;
			}
			if (index2 == -1L)
			{
				this.index2 = series.count - 1L;
			}
			else
			{
				this.index2 = index2;
			}
			this.current = index1;
		}
		public DataObject GetNext()
		{
			if (this.current > this.index2)
			{
				return null;
			}
			DataSeries arg_28_0 = this.series;
			long index;
			this.current = (index = this.current) + 1L;
			return arg_28_0.Get(index);
		}
	}
}
