using System;
using System.IO;
namespace SmartQuant
{
	public class ObjectKey : IComparable<ObjectKey>
	{
		internal DataFile file;
		internal object obj;
		internal string label = "OKey";
		internal bool deleted;
		internal DateTime dateTime;
		internal long position = -1L;
		internal int keyLength = -1;
		internal int objLength = -1;
		internal int recLength = -1;
		internal byte compressionMethod = 1;
		internal byte compressionLevel = 1;
		internal byte typeId;
		internal string name;
		protected internal bool changed;
		public byte TypeId
		{
			get
			{
				return this.typeId;
			}
		}
		public ObjectKey()
		{
		}
		public ObjectKey(DataFile file, string name = null, object obj = null)
		{
			this.name = name;
			this.obj = obj;
			if (file != null)
			{
				this.compressionLevel = file.compressionLevel;
				this.compressionMethod = file.compressionMethod;
				this.Init(file);
			}
		}
		internal void Init(DataFile file)
		{
			this.file = file;
			if (this.obj != null)
			{
				ObjectStreamer objectStreamer;
				file.streamerManager.streamerByType.TryGetValue(this.obj.GetType(), out objectStreamer);
				if (objectStreamer != null)
				{
					this.typeId = objectStreamer.typeId;
					return;
				}
				Console.WriteLine("ObjectKey::Init Can not find streamer for object of type " + this.obj.GetType());
			}
		}
		public virtual object GetObject()
		{
			if (this.obj != null)
			{
				return this.obj;
			}
			if (this.objLength == -1)
			{
				return null;
			}
			MemoryStream input = new MemoryStream(this.ReadObjectData());
			BinaryReader reader = new BinaryReader(input);
			ObjectStreamer objectStreamer = this.file.streamerManager.streamerByTypeId[(int)this.typeId];
			this.obj = objectStreamer.Read(reader);
			if (this.typeId == 101)
			{
				((DataSeries)this.obj).Init(this.file, this);
			}
			return this.obj;
		}
		internal byte[] ReadObjectData()
		{
			byte[] array = new byte[this.objLength];
			this.file.ReadBuffer(array, this.position + (long)this.keyLength, this.objLength);
			if (this.compressionLevel == 0)
			{
				return array;
			}
			QuickLZ quickLZ = new QuickLZ();
			return quickLZ.Decompress(array);
		}
		internal byte[] WriteObjectData()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(memoryStream);
			ObjectStreamer objectStreamer = this.file.streamerManager.streamerByType[this.obj.GetType()];
			objectStreamer.Write(writer, this.obj);
			byte[] array = memoryStream.ToArray();
			if (this.compressionLevel == 0)
			{
				return array;
			}
			QuickLZ quickLZ = new QuickLZ();
			return quickLZ.Compress(array);
		}
		internal virtual void Write(BinaryWriter writer)
		{
			byte[] array = this.WriteObjectData();
			this.keyLength = 37 + this.name.Length + 1;
			this.objLength = array.Length;
			if (this.recLength == -1)
			{
				this.recLength = this.keyLength + this.objLength;
			}
			writer.Write(this.label);
			writer.Write(this.deleted);
			writer.Write(this.dateTime.Ticks);
			writer.Write(this.position);
			writer.Write(this.keyLength);
			writer.Write(this.objLength);
			writer.Write(this.recLength);
			writer.Write(this.compressionMethod);
			writer.Write(this.compressionLevel);
			writer.Write(this.typeId);
			writer.Write(this.name);
			writer.Write(array, 0, array.Length);
		}
		internal virtual void WriteKey(BinaryWriter writer)
		{
			writer.Write(this.label);
			writer.Write(this.deleted);
			writer.Write(this.dateTime.Ticks);
			writer.Write(this.position);
			writer.Write(this.keyLength);
			writer.Write(this.objLength);
			writer.Write(this.recLength);
			writer.Write(this.compressionMethod);
			writer.Write(this.compressionLevel);
			writer.Write(this.typeId);
			writer.Write(this.name);
		}
		internal virtual void Read(BinaryReader reader, bool readLabel = true)
		{
			if (readLabel)
			{
				this.label = reader.ReadString();
				if (this.label != "OKey")
				{
					Console.WriteLine("ObjectKey::Read This is not ObjectKey! label = " + this.label);
				}
			}
			this.deleted = reader.ReadBoolean();
			this.dateTime = new DateTime(reader.ReadInt64());
			this.position = reader.ReadInt64();
			this.keyLength = reader.ReadInt32();
			this.objLength = reader.ReadInt32();
			this.recLength = reader.ReadInt32();
			this.compressionMethod = reader.ReadByte();
			this.compressionLevel = reader.ReadByte();
			this.typeId = reader.ReadByte();
			this.name = reader.ReadString();
		}
		public virtual void Dump()
		{
			Console.WriteLine(string.Concat(new object[]
			{
				this.name,
				" of typeId ",
				this.typeId,
				" (",
				this.file.streamerManager.streamerByTypeId[(int)this.typeId].type,
				") position = ",
				this.position
			}));
		}
		public int CompareTo(ObjectKey other)
		{
			if (other == null)
			{
				return 1;
			}
			return this.recLength.CompareTo(other.recLength);
		}
	}
}
