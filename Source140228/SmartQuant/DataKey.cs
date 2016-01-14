using System;
using System.IO;
namespace SmartQuant
{
	internal class DataKey : ObjectKey
	{
		internal DataObject[] objects;
		internal int size = 10000;
		internal int count;
		internal DateTime dateTime1;
		internal DateTime dateTime2;
		internal int number = -1;
		internal long index1;
		internal long index2;
		internal long prev = -1L;
		internal long next = -1L;
		public DataKey(DataFile file, object obj = null, long prev = -1L, long next = -1L) : base(file, "", obj)
		{
			this.label = "DKey";
			this.keyLength = 77;
			this.prev = prev;
			this.next = next;
		}
		public void AddObject(DataObject obj)
		{
			if (this.count == this.size)
			{
				Console.WriteLine("DataKey::Add Can not add object. Buffer is full.");
				return;
			}
			if (this.objects == null)
			{
				this.objects = new DataObject[this.size];
			}
			if (this.count == 0)
			{
				this.objects[this.count++] = obj;
				this.dateTime1 = obj.dateTime;
				this.dateTime2 = obj.dateTime;
			}
			else
			{
				if (obj.dateTime >= this.objects[this.count - 1].dateTime)
				{
					this.objects[this.count++] = obj;
					this.dateTime2 = obj.dateTime;
				}
				else
				{
					int num = this.count;
					while (true)
					{
						this.objects[num] = this.objects[num - 1];
						if (obj.dateTime >= this.objects[num].dateTime || num == 1)
						{
							break;
						}
						num--;
					}
					this.objects[num - 1] = obj;
					if (num == 1)
					{
						this.dateTime1 = obj.dateTime;
					}
					this.count++;
				}
			}
			this.index2 += 1L;
			this.changed = true;
		}
		public void UpdateObject(int index, DataObject obj)
		{
			this.objects[index] = obj;
			this.changed = true;
		}
		public void RemoveObject(long index)
		{
			if (this.objects == null)
			{
				this.GetObjects();
			}
			for (long num = index; num < (long)(this.count - 1); num += 1L)
			{
				checked
				{
					this.objects[(int)((IntPtr)num)] = this.objects[(int)((IntPtr)unchecked(num + 1L))];
				}
			}
			this.count--;
			this.changed = true;
			if (this.count == 0)
			{
				return;
			}
			if (index == 0L)
			{
				this.dateTime1 = this.objects[0].dateTime;
			}
			if (index == (long)this.count)
			{
				this.dateTime2 = this.objects[this.count - 1].dateTime;
			}
		}
		public DataObject[] GetObjects()
		{
			if (this.objects != null)
			{
				return this.objects;
			}
			this.objects = new DataObject[this.size];
			if (this.objLength == -1)
			{
				return this.objects;
			}
			MemoryStream input = new MemoryStream(base.ReadObjectData());
			BinaryReader reader = new BinaryReader(input);
			for (int i = 0; i < this.count; i++)
			{
				this.objects[i] = (DataObject)this.file.streamerManager.Deserialize(reader);
			}
			return this.objects;
		}
		public DataObject GetObject(int index)
		{
			return this.GetObjects()[index];
		}
		public DataObject GetObject(DateTime dateTime)
		{
			if (this.objects == null)
			{
				this.GetObjects();
			}
			for (int i = 0; i < this.count; i++)
			{
				if (this.objects[i].dateTime >= dateTime)
				{
					return this.objects[i];
				}
			}
			return null;
		}
		public int GetIndex(DateTime dateTime, SearchOption option = SearchOption.Next)
		{
			if (this.objects == null)
			{
				this.GetObjects();
			}
			for (int i = 0; i < this.count; i++)
			{
				if (this.objects[i].dateTime >= dateTime)
				{
					switch (option)
					{
					case SearchOption.Next:
						return i;
					case SearchOption.Prev:
						if (this.objects[i].dateTime == dateTime)
						{
							return i;
						}
						return i - 1;
					case SearchOption.Exact:
						if (this.objects[i].dateTime != dateTime)
						{
							return -1;
						}
						return i;
					default:
						Console.WriteLine("DataKey::GetIndex Unknown search option: " + option);
						break;
					}
				}
			}
			return -1;
		}
		internal override void Write(BinaryWriter writer)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter writer2 = new BinaryWriter(memoryStream);
			for (int i = 0; i < this.count; i++)
			{
				this.file.streamerManager.Serialize(writer2, this.objects[i]);
			}
			byte[] array = memoryStream.ToArray();
			if (this.compressionLevel != 0)
			{
				QuickLZ quickLZ = new QuickLZ();
				array = quickLZ.Compress(array);
			}
			this.keyLength = 77;
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
			writer.Write(this.size);
			writer.Write(this.count);
			writer.Write(this.dateTime1.Ticks);
			writer.Write(this.dateTime2.Ticks);
			writer.Write(this.prev);
			writer.Write(this.next);
			writer.Write(array, 0, array.Length);
		}
		internal override void WriteKey(BinaryWriter writer)
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
			writer.Write(this.size);
			writer.Write(this.count);
			writer.Write(this.dateTime1.Ticks);
			writer.Write(this.dateTime2.Ticks);
			writer.Write(this.prev);
			writer.Write(this.next);
		}
		internal override void Read(BinaryReader reader, bool readLabel = true)
		{
			if (readLabel)
			{
				this.label = reader.ReadString();
				if (this.label != "DKey")
				{
					Console.WriteLine("ObjectKey::ReadKey This is not DataKey! label = " + this.label);
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
			this.size = reader.ReadInt32();
			this.count = reader.ReadInt32();
			this.dateTime1 = new DateTime(reader.ReadInt64());
			this.dateTime2 = new DateTime(reader.ReadInt64());
			this.prev = reader.ReadInt64();
			this.next = reader.ReadInt64();
		}
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"position = ",
				this.position,
				" prev =  ",
				this.prev,
				" next = ",
				this.next,
				" number ",
				this.number,
				" size = ",
				this.size,
				" count = ",
				this.count,
				" ",
				this.changed
			});
		}
	}
}
