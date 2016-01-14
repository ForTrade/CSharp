using System;
using System.IO;
namespace SmartQuant
{
	public class BidStreamer : ObjectStreamer
	{
		public BidStreamer()
		{
			this.typeId = 2;
			this.type = typeof(Bid);
		}
		public override object Read(BinaryReader reader)
		{
			if (reader.ReadByte() == 0)
			{
				return new Bid(new DateTime(reader.ReadInt64()), reader.ReadByte(), reader.ReadInt32(), reader.ReadDouble(), reader.ReadInt32());
			}
			return new Bid(new DateTime(reader.ReadInt64()), new DateTime(reader.ReadInt64()), reader.ReadByte(), reader.ReadInt32(), reader.ReadDouble(), reader.ReadInt32());
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			Bid bid = (Bid)obj;
			byte b = 0;
			if (bid.exchangeDateTime.Ticks != 0L)
			{
				b = 1;
			}
			writer.Write(b);
			writer.Write(bid.dateTime.Ticks);
			if (b == 1)
			{
				writer.Write(bid.exchangeDateTime.Ticks);
			}
			writer.Write(bid.providerId);
			writer.Write(bid.instrumentId);
			writer.Write(bid.price);
			writer.Write(bid.size);
		}
	}
}
