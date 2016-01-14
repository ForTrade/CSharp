using System;
namespace SmartQuant
{
	internal class DataSeriesObject
	{
		internal DataSeries series;
		internal DataObject obj;
		internal IEventQueue queue;
		internal long index1;
		internal long index2;
		internal long current;
		internal long count;
		internal int progressDelta;
		internal int progressCount;
		internal int progressPercent;
		internal long Count
		{
			get
			{
				return this.index2 - this.index1 + 1L;
			}
		}
		internal DataSeriesObject(DataSeries series, DateTime dateTime1, DateTime dateTime2, IEventQueue queue)
		{
			this.series = series;
			this.queue = queue;
			if (dateTime1 == DateTime.MinValue || dateTime1 < series.DateTime1)
			{
				this.index1 = 0L;
			}
			else
			{
				this.index1 = series.GetIndex(dateTime1, SearchOption.Next);
			}
			if (dateTime2 == DateTime.MaxValue || dateTime2 > series.DateTime2)
			{
				this.index2 = series.count - 1L;
			}
			else
			{
				this.index2 = series.GetIndex(dateTime2, SearchOption.Prev);
			}
			this.current = this.index1;
			this.obj = series[this.current];
			this.progressDelta = (int)Math.Ceiling((double)this.Count / 100.0);
			this.progressCount = this.progressDelta;
			this.progressPercent = 0;
		}
		internal bool Enqueue()
		{
			if (!this.queue.IsFull())
			{
				this.queue.Write(this.obj);
				this.current += 1L;
				if (this.current <= this.index2)
				{
					this.obj = this.series[this.current];
				}
				else
				{
					this.obj = null;
				}
				this.count += 1L;
				if (this.count == (long)this.progressCount)
				{
					this.progressCount += this.progressDelta;
					this.progressPercent++;
					this.queue.Enqueue(new OnSimulatorProgress((long)this.progressCount, this.progressPercent));
				}
				return true;
			}
			return false;
		}
	}
}
