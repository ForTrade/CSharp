using System;
using System.IO;
namespace SmartQuant
{
	public class TradeStreamer : ObjectStreamer
	{
		public TradeStreamer()
		{
			this.typeId = 4;
			this.type = typeof(Trade);
		}
		public override object Read(BinaryReader reader)
		{
			if (reader.ReadByte() == 0)
			{
				return new Trade(new DateTime(reader.ReadInt64()), reader.ReadByte(), reader.ReadInt32(), reader.ReadDouble(), reader.ReadInt32());
			}
			return new Trade(new DateTime(reader.ReadInt64()), new DateTime(reader.ReadInt64()), reader.ReadByte(), reader.ReadInt32(), reader.ReadDouble(), reader.ReadInt32());
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			Trade trade = (Trade)obj;
			byte b = 0;
			if (trade.exchangeDateTime.Ticks != 0L)
			{
				b = 1;
			}
			writer.Write(b);
			writer.Write(trade.dateTime.Ticks);
			if (b == 1)
			{
				writer.Write(trade.exchangeDateTime.Ticks);
			}
			writer.Write(trade.providerId);
			writer.Write(trade.instrumentId);
			writer.Write(trade.price);
			writer.Write(trade.size);
		}
	}
}
