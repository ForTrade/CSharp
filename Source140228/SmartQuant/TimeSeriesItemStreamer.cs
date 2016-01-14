using System;
using System.IO;
namespace SmartQuant
{
	public class TimeSeriesItemStreamer : ObjectStreamer
	{
		public TimeSeriesItemStreamer()
		{
			this.typeId = 11;
			this.type = typeof(TimeSeriesItem);
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			TimeSeriesItem timeSeriesItem = (TimeSeriesItem)obj;
			writer.Write(0);
			writer.Write(timeSeriesItem.dateTime.ToBinary());
			writer.Write(timeSeriesItem.value);
		}
		public override object Read(BinaryReader reader)
		{
			TimeSeriesItem timeSeriesItem = new TimeSeriesItem();
			reader.ReadByte();
			timeSeriesItem.dateTime = DateTime.FromBinary(reader.ReadInt64());
			timeSeriesItem.value = reader.ReadDouble();
			return timeSeriesItem;
		}
	}
}
