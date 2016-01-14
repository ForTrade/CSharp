using System;
using System.Collections.Generic;
using System.IO;
namespace SmartQuant
{
	public class ObjectKeyListStreamer : ObjectStreamer
	{
		public ObjectKeyListStreamer()
		{
			this.typeId = 102;
			this.type = typeof(ObjectKeyList);
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			ObjectKeyList objectKeyList = (ObjectKeyList)obj;
			Dictionary<string, ObjectKey> keys = objectKeyList.keys;
			writer.Write(0);
			writer.Write(keys.Count);
			foreach (ObjectKey current in keys.Values)
			{
				current.WriteKey(writer);
			}
		}
		public override object Read(BinaryReader reader)
		{
			Dictionary<string, ObjectKey> dictionary = new Dictionary<string, ObjectKey>();
			reader.ReadByte();
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				ObjectKey objectKey = new ObjectKey();
				objectKey.Read(reader, true);
				dictionary.Add(objectKey.name, objectKey);
			}
			return new ObjectKeyList(dictionary);
		}
	}
}
