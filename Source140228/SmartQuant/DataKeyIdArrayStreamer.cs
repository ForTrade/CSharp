using System;
using System.IO;
namespace SmartQuant
{
	public class DataKeyIdArrayStreamer : ObjectStreamer
	{
		public DataKeyIdArrayStreamer()
		{
			this.typeId = 105;
			this.type = typeof(DataKeyIdArray);
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			DataKeyIdArray dataKeyIdArray = (DataKeyIdArray)obj;
			IdArray<DataKey> keys = dataKeyIdArray.keys;
			writer.Write(0);
			writer.Write(keys.Size);
			for (int i = 0; i < keys.Size; i++)
			{
				if (keys[i] != null)
				{
					writer.Write(i);
					keys[i].WriteKey(writer);
				}
			}
			writer.Write(-1);
		}
		public override object Read(BinaryReader reader)
		{
			reader.ReadByte();
			int size = reader.ReadInt32();
			IdArray<DataKey> idArray = new IdArray<DataKey>(size);
			while (true)
			{
				int num = reader.ReadInt32();
				if (num == -1)
				{
					break;
				}
				DataKey dataKey = new DataKey(null, null, -1L, -1L);
				dataKey.Read(reader, true);
				idArray.Add(num, dataKey);
			}
			return new DataKeyIdArray(idArray);
		}
	}
}
