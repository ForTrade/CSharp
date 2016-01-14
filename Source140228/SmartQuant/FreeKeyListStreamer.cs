using System;
using System.Collections.Generic;
using System.IO;
namespace SmartQuant
{
	public class FreeKeyListStreamer : ObjectStreamer
	{
		public FreeKeyListStreamer()
		{
			this.typeId = 103;
			this.type = typeof(FreeKeyList);
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			FreeKeyList freeKeyList = (FreeKeyList)obj;
			List<FreeKey> keys = freeKeyList.keys;
			writer.Write(0);
			writer.Write(keys.Count);
			foreach (FreeKey current in keys)
			{
				current.Write(writer);
			}
		}
		public override object Read(BinaryReader reader)
		{
			List<FreeKey> list = new List<FreeKey>();
			reader.ReadByte();
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				FreeKey freeKey = new FreeKey();
				freeKey.Read(reader, true);
				list.Add(freeKey);
			}
			return new FreeKeyList(list);
		}
	}
}
