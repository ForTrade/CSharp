using System;
using System.IO;
namespace SmartQuant
{
	public class Level2UpdateStreamer : ObjectStreamer
	{
		public Level2UpdateStreamer()
		{
			this.typeId = 9;
			this.type = typeof(Level2Update);
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			Level2Update level2Update = (Level2Update)obj;
			writer.Write(0);
			writer.Write(level2Update.dateTime.ToBinary());
			writer.Write(level2Update.providerId);
			writer.Write(level2Update.instrumentId);
			writer.Write(level2Update.entries.Length);
			Level2[] entries = level2Update.entries;
			for (int i = 0; i < entries.Length; i++)
			{
				Level2 obj2 = entries[i];
				this.streamerManager.Serialize(writer, obj2);
			}
		}
		public override object Read(BinaryReader reader)
		{
			Level2Update level2Update = new Level2Update();
			reader.ReadByte();
			level2Update.dateTime = DateTime.FromBinary(reader.ReadInt64());
			level2Update.providerId = reader.ReadByte();
			level2Update.instrumentId = reader.ReadInt32();
			int num = reader.ReadInt32();
			level2Update.entries = new Level2[num];
			for (int i = 0; i < num; i++)
			{
				level2Update.entries[i] = (Level2)this.streamerManager.Deserialize(reader);
			}
			return level2Update;
		}
	}
}
