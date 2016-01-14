using System;
using System.IO;
namespace SmartQuant
{
	public class Level2SnapshotStreamer : ObjectStreamer
	{
		public Level2SnapshotStreamer()
		{
			this.typeId = 8;
			this.type = typeof(Level2Snapshot);
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			Level2Snapshot level2Snapshot = (Level2Snapshot)obj;
			writer.Write(0);
			writer.Write(level2Snapshot.dateTime.ToBinary());
			writer.Write(level2Snapshot.providerId);
			writer.Write(level2Snapshot.instrumentId);
			writer.Write(level2Snapshot.bids.Length);
			Bid[] bids = level2Snapshot.bids;
			for (int i = 0; i < bids.Length; i++)
			{
				Bid obj2 = bids[i];
				this.streamerManager.Serialize(writer, obj2);
			}
			writer.Write(level2Snapshot.asks.Length);
			Ask[] asks = level2Snapshot.asks;
			for (int j = 0; j < asks.Length; j++)
			{
				Ask obj3 = asks[j];
				this.streamerManager.Serialize(writer, obj3);
			}
		}
		public override object Read(BinaryReader reader)
		{
			Level2Snapshot level2Snapshot = new Level2Snapshot();
			reader.ReadByte();
			level2Snapshot.dateTime = DateTime.FromBinary(reader.ReadInt64());
			level2Snapshot.providerId = reader.ReadByte();
			level2Snapshot.instrumentId = reader.ReadInt32();
			int num = reader.ReadInt32();
			level2Snapshot.bids = new Bid[num];
			for (int i = 0; i < num; i++)
			{
				level2Snapshot.bids[i] = (Bid)this.streamerManager.Deserialize(reader);
			}
			int num2 = reader.ReadInt32();
			level2Snapshot.asks = new Ask[num2];
			for (int j = 0; j < num2; j++)
			{
				level2Snapshot.asks[j] = (Ask)this.streamerManager.Deserialize(reader);
			}
			return level2Snapshot;
		}
	}
}
