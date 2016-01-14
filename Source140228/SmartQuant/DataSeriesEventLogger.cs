using System;
namespace SmartQuant
{
	public class DataSeriesEventLogger : EventLogger
	{
		private DataSeries series;
		private DateTime dateTime;
		private IdArray<bool> filter = new IdArray<bool>(256);
		public DataSeriesEventLogger(Framework framework, DataSeries series) : base(framework, "DataSeriesEventLogger")
		{
			this.series = series;
		}
		public DataSeriesEventLogger(DataSeries series) : base(Framework.Current, "DataSeriesEventLogger")
		{
			this.series = series;
		}
		public void Enable(byte typeId)
		{
			this.filter[(int)typeId] = true;
		}
		public void Disable(byte typeId)
		{
			this.filter[(int)typeId] = true;
		}
		public override void OnEvent(Event e)
		{
			if (this.filter[(int)e.TypeId])
			{
				if (e.dateTime < this.dateTime)
				{
					Console.WriteLine(string.Concat(new object[]
					{
						"!",
						e,
						" = ",
						e.dateTime,
						" <> ",
						this.dateTime
					}));
					return;
				}
				this.dateTime = e.dateTime;
				this.series.Add((DataObject)e);
			}
		}
	}
}
