using System;
using System.IO;
namespace SmartQuant
{
	public class TickStreamer : ObjectStreamer
	{
		public TickStreamer()
		{
			this.typeId = 1;
			this.type = typeof(Tick);
		}
		public override object Read(BinaryReader reader)
		{
			if (reader.ReadByte() == 0)
			{
				return new Tick(new DateTime(reader.ReadInt64()), reader.ReadByte(), reader.ReadInt32(), reader.ReadDouble(), reader.ReadInt32());
			}
			return new Tick(new DateTime(reader.ReadInt64()), new DateTime(reader.ReadInt64()), reader.ReadByte(), reader.ReadInt32(), reader.ReadDouble(), reader.ReadInt32());
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			Tick tick = (Tick)obj;
			byte b = 0;
			if (tick.exchangeDateTime.Ticks != 0L)
			{
				b = 1;
			}
			writer.Write(b);
			writer.Write(tick.dateTime.Ticks);
			if (b == 1)
			{
				writer.Write(tick.exchangeDateTime.Ticks);
			}
			writer.Write(tick.providerId);
			writer.Write(tick.instrumentId);
			writer.Write(tick.price);
			writer.Write(tick.size);
		}
	}
}
