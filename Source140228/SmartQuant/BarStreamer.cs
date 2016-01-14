using System;
using System.IO;
namespace SmartQuant
{
	public class BarStreamer : ObjectStreamer
	{
		public BarStreamer()
		{
			this.typeId = 6;
			this.type = typeof(Bar);
		}
		public override object Read(BinaryReader reader)
		{
			reader.ReadByte();
			Bar bar = new Bar();
			bar.dateTime = new DateTime(reader.ReadInt64());
			bar.openDateTime = new DateTime(reader.ReadInt64());
			bar.instrumentId = reader.ReadInt32();
			bar.size = reader.ReadInt64();
			bar.high = reader.ReadDouble();
			bar.low = reader.ReadDouble();
			bar.open = reader.ReadDouble();
			bar.close = reader.ReadDouble();
			bar.volume = reader.ReadInt64();
			int num = reader.ReadInt32();
			if (num != 0)
			{
				bar.fields = new IdArray<double>(num);
				for (int i = 0; i < num; i++)
				{
					bar.fields[i] = reader.ReadDouble();
				}
			}
			return bar;
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			byte value = 0;
			writer.Write(value);
			Bar bar = (Bar)obj;
			writer.Write(bar.dateTime.Ticks);
			writer.Write(bar.openDateTime.Ticks);
			writer.Write(bar.instrumentId);
			writer.Write(bar.size);
			writer.Write(bar.high);
			writer.Write(bar.low);
			writer.Write(bar.open);
			writer.Write(bar.close);
			writer.Write(bar.volume);
			if (bar.fields != null)
			{
				writer.Write(bar.fields.Size);
				for (int i = 0; i < bar.fields.Size; i++)
				{
					writer.Write(bar.fields[i]);
				}
				return;
			}
			int value2 = 0;
			writer.Write(value2);
		}
	}
}
