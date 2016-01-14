using System;
using System.IO;
namespace SmartQuant
{
	public class FieldListStreamer : ObjectStreamer
	{
		public FieldListStreamer()
		{
			this.typeId = 19;
			this.type = typeof(FieldList);
		}
		public override object Read(BinaryReader reader)
		{
			reader.ReadByte();
			FieldList fieldList = new FieldList();
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				int id = reader.ReadInt32();
				double value = reader.ReadDouble();
				fieldList.fields[id] = value;
			}
			return fieldList;
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			byte value = 0;
			writer.Write(value);
			FieldList fieldList = obj as FieldList;
			int num = 0;
			for (int i = 0; i < fieldList.fields.Size; i++)
			{
				if (fieldList.fields[i] != 0.0)
				{
					num++;
				}
			}
			writer.Write(num);
			for (int j = 0; j < fieldList.fields.Size; j++)
			{
				if (fieldList.fields[j] != 0.0)
				{
					writer.Write(j);
					writer.Write(fieldList.fields[j]);
				}
			}
		}
	}
}
