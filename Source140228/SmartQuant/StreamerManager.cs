using System;
using System.Collections.Generic;
using System.IO;
namespace SmartQuant
{
	public class StreamerManager
	{
		internal IdArray<ObjectStreamer> streamerByTypeId = new IdArray<ObjectStreamer>(1000);
		internal Dictionary<Type, ObjectStreamer> streamerByType = new Dictionary<Type, ObjectStreamer>();
		public StreamerManager()
		{
			this.Add(new FreeKeyListStreamer());
			this.Add(new ObjectKeyListStreamer());
			this.Add(new DataSeriesStreamer());
			this.Add(new DataKeyIdArrayStreamer());
		}
		public void Add(ObjectStreamer streamer)
		{
			streamer.streamerManager = this;
			this.streamerByTypeId[(int)streamer.typeId] = streamer;
			this.streamerByType[streamer.type] = streamer;
		}
		public object Deserialize(BinaryReader reader)
		{
			int id = (int)reader.ReadByte();
			ObjectStreamer objectStreamer = this.streamerByTypeId[id];
			return objectStreamer.Read(reader);
		}
		public void Serialize(BinaryWriter writer, object obj)
		{
			Type type = obj.GetType();
			ObjectStreamer objectStreamer = this.streamerByType[type];
			writer.Write(objectStreamer.typeId);
			objectStreamer.Write(writer, obj);
		}
	}
}
