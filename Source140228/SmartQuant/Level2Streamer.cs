using System;
using System.IO;
namespace SmartQuant
{
	public class Level2Streamer : ObjectStreamer
	{
		public Level2Streamer()
		{
			this.typeId = 7;
			this.type = typeof(Level2);
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			Level2 level = (Level2)obj;
			writer.Write(0);
			writer.Write(level.dateTime.ToBinary());
			writer.Write(level.providerId);
			writer.Write(level.instrumentId);
			writer.Write(level.price);
			writer.Write(level.size);
			writer.Write((byte)level.side);
			writer.Write((byte)level.action);
			writer.Write(level.position);
		}
		public override object Read(BinaryReader reader)
		{
			Level2 level = new Level2();
			reader.ReadByte();
			level.dateTime = DateTime.FromBinary(reader.ReadInt64());
			level.providerId = reader.ReadByte();
			level.instrumentId = reader.ReadInt32();
			level.price = reader.ReadDouble();
			level.size = reader.ReadInt32();
			level.side = (Level2Side)reader.ReadByte();
			level.action = (Level2UpdateAction)reader.ReadByte();
			level.position = reader.ReadInt32();
			return level;
		}
	}
}
