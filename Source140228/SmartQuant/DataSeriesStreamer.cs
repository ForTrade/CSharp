using System;
using System.IO;
namespace SmartQuant
{
	public class DataSeriesStreamer : ObjectStreamer
	{
		public DataSeriesStreamer()
		{
			this.typeId = 101;
			this.type = typeof(DataSeries);
		}
		public override object Read(BinaryReader reader)
		{
			reader.ReadByte();
			return new DataSeries
			{
				count = reader.ReadInt64(),
				buffer_count = reader.ReadInt32(),
				cachePosition = reader.ReadInt64(),
				dateTime2 = new DateTime(reader.ReadInt64()),
				dateTime1 = new DateTime(reader.ReadInt64()),
				position1 = reader.ReadInt64(),
				position2 = reader.ReadInt64(),
				name = reader.ReadString()
			};
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			byte value = 0;
			writer.Write(value);
			DataSeries dataSeries = obj as DataSeries;
			writer.Write(dataSeries.count);
			writer.Write(dataSeries.buffer_count);
			writer.Write(dataSeries.cachePosition);
			writer.Write(dataSeries.dateTime2.Ticks);
			writer.Write(dataSeries.dateTime1.Ticks);
			writer.Write(dataSeries.position1);
			writer.Write(dataSeries.position2);
			writer.Write(dataSeries.name);
		}
	}
}
