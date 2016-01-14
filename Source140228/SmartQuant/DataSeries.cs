using System;
using System.IO;
namespace SmartQuant
{
	public class DataSeries
	{
		private const int bufferSize = 10000;
		internal string name;
		internal DataFile file;
		internal ObjectKey key;
		internal long count;
		internal int buffer_count;
		internal DateTime dateTime1;
		internal DateTime dateTime2;
		internal long position1 = -1L;
		internal long position2 = -1L;
		private bool isOpenRead;
		private bool isOpenWrite;
		private DataKey readKey;
		private DataKey writeKey;
		private DataKey deleteKey;
		private DataKey insertKey;
		private IdArray<DataKey> cache;
		internal ObjectKey cacheKey;
		internal long cachePosition = -1L;
		internal bool cacheObjects;
		internal bool changed;
		public string Name
		{
			get
			{
				return this.name;
			}
		}
		public long Count
		{
			get
			{
				return this.count;
			}
		}
		public DateTime DateTime1
		{
			get
			{
				return this.dateTime1;
			}
		}
		public DateTime DateTime2
		{
			get
			{
				return this.dateTime2;
			}
		}
		public bool CacheObjects
		{
			get
			{
				return this.cacheObjects;
			}
			set
			{
				this.cacheObjects = value;
			}
		}
		public DataObject this[long index]
		{
			get
			{
				return this.Get(index);
			}
		}
		public DataObject this[DateTime dateTime]
		{
			get
			{
				return this.Get(dateTime);
			}
		}
		public DataSeries()
		{
		}
		public DataSeries(string name)
		{
			this.name = name;
		}
		internal void Init(DataFile file, ObjectKey key)
		{
			this.file = file;
			this.key = key;
			key.compressionLevel = 0;
			key.compressionMethod = 0;
			if (this.cachePosition == -1L)
			{
				if (this.buffer_count < 4096)
				{
					this.cache = new IdArray<DataKey>(4096);
				}
				else
				{
					this.cache = new IdArray<DataKey>(this.buffer_count);
				}
				this.cacheKey = new ObjectKey(file, "", new DataKeyIdArray(this.cache));
			}
		}
		private void OpenRead()
		{
			if (this.isOpenRead)
			{
				Console.WriteLine("DataSeries::OpenRead already read open");
				return;
			}
			if (this.cache == null)
			{
				this.ReadCache();
			}
			this.isOpenRead = true;
		}
		private void OpenWrite()
		{
			if (this.isOpenWrite)
			{
				Console.WriteLine("DataSeries::OpenWrite already write open");
				return;
			}
			if (this.cache == null)
			{
				this.ReadCache();
			}
			if (this.buffer_count != 0 && this.cache[this.buffer_count - 1] != null)
			{
				this.writeKey = this.cache[this.buffer_count - 1];
				this.writeKey.GetObjects();
			}
			else
			{
				if (this.position2 != -1L)
				{
					this.writeKey = this.ReadKey(this.position2);
					this.writeKey.number = this.buffer_count - 1;
					this.writeKey.GetObjects();
				}
				else
				{
					this.writeKey = new DataKey(this.file, null, -1L, -1L);
					this.writeKey.number = 0;
					this.writeKey.changed = true;
					this.buffer_count = 1;
				}
				this.cache[this.writeKey.number] = this.writeKey;
			}
			this.isOpenWrite = true;
		}
		private void ReadCache()
		{
			this.cacheKey = this.file.ReadKey(this.cachePosition, 38);
			this.cache = ((DataKeyIdArray)this.cacheKey.GetObject()).keys;
			for (int i = 0; i < this.cache.Size; i++)
			{
				if (this.cache[i] != null)
				{
					this.cache[i].file = this.file;
					this.cache[i].number = i;
				}
			}
		}
		private void WriteCache()
		{
			if (this.cacheKey == null)
			{
				this.cacheKey = new ObjectKey(this.file, "", new DataKeyIdArray(this.cache));
			}
			this.file.WriteKey(this.cacheKey);
			this.cachePosition = this.cacheKey.position;
		}
		public void Update(long index, DataObject obj)
		{
			DataObject dataObject = this.Get(index);
			if (dataObject.dateTime != obj.dateTime)
			{
				Console.WriteLine("DataSeries::Update Can not update object with different datetime");
				return;
			}
			bool flag = this.readKey.changed;
			this.readKey.UpdateObject((int)(index - this.readKey.index1), obj);
			if (!flag)
			{
				this.WriteKey(this.readKey);
			}
			this.file.isModified = true;
		}
		public void Add(DataObject obj)
		{
			if (obj.dateTime.Ticks == 0L)
			{
				Console.WriteLine("DataSeries::Add Error: can not add object with DateTime = 0");
				return;
			}
			if (!this.isOpenWrite)
			{
				this.OpenWrite();
			}
			this.count += 1L;
			if (this.count == 1L)
			{
				this.dateTime1 = obj.dateTime;
				this.dateTime2 = obj.dateTime;
			}
			else
			{
				if (obj.dateTime < this.dateTime2)
				{
					this.Insert(obj);
					return;
				}
				this.dateTime2 = obj.DateTime;
			}
			this.writeKey.AddObject(obj);
			if (this.writeKey.count == this.writeKey.size)
			{
				this.WriteKey(this.writeKey);
				if (!this.cacheObjects && this.writeKey != this.readKey && this.writeKey != this.insertKey && this.writeKey != this.deleteKey)
				{
					this.writeKey.objects = null;
				}
				this.writeKey = new DataKey(this.file, null, this.writeKey.position, -1L);
				this.writeKey.number = this.buffer_count;
				this.writeKey.index1 = this.count;
				this.writeKey.index2 = this.count;
				this.writeKey.changed = true;
				this.buffer_count++;
				this.cache[this.writeKey.number] = this.writeKey;
			}
			else
			{
				this.changed = true;
			}
			this.file.isModified = true;
		}
		private void Insert(DataObject obj)
		{
			if (obj.dateTime >= this.writeKey.dateTime1 && obj.dateTime <= this.writeKey.dateTime2)
			{
				this.writeKey.AddObject(obj);
				if (this.writeKey.count == this.writeKey.size)
				{
					this.WriteKey(this.writeKey);
					this.writeKey = new DataKey(this.file, null, this.writeKey.position, -1L);
					this.writeKey.number = this.buffer_count;
					this.writeKey.index1 = this.count;
					this.writeKey.index2 = this.count;
					this.writeKey.changed = true;
					this.buffer_count++;
					this.cache[this.writeKey.number] = this.writeKey;
				}
				else
				{
					this.changed = true;
				}
				this.file.isModified = true;
				return;
			}
			DataKey dataKey = this.GetKey(obj.dateTime, this.insertKey);
			if (this.insertKey == null)
			{
				this.insertKey = dataKey;
			}
			else
			{
				if (this.insertKey != dataKey)
				{
					if (this.insertKey.changed)
					{
						this.WriteKey(this.insertKey);
					}
					if (!this.cacheObjects && this.insertKey != this.readKey && this.insertKey != this.writeKey && this.insertKey != this.deleteKey)
					{
						this.insertKey.objects = null;
					}
					this.insertKey = dataKey;
				}
			}
			this.insertKey.GetObjects();
			if (this.insertKey.count < this.insertKey.size)
			{
				this.insertKey.AddObject(obj);
				if (this.insertKey.count == this.insertKey.size)
				{
					this.WriteKey(this.insertKey);
				}
			}
			else
			{
				dataKey = new DataKey(this.file, null, -1L, -1L);
				int index = this.insertKey.GetIndex(obj.dateTime, SearchOption.Next);
				for (int i = index; i < this.insertKey.count; i++)
				{
					dataKey.AddObject(this.insertKey.objects[i]);
					this.insertKey.objects[i] = null;
				}
				this.insertKey.count = index;
				this.insertKey.index2 = this.insertKey.index1 + (long)this.insertKey.count - 1L;
				if (this.insertKey.count != 0)
				{
					this.insertKey.dateTime2 = this.insertKey.objects[this.insertKey.count - 1].dateTime;
				}
				this.insertKey.AddObject(obj);
				this.InsertKey(dataKey, this.insertKey);
			}
			if (this.readKey != null && this.readKey.number > this.insertKey.number)
			{
				this.readKey.index1 += 1L;
				this.readKey.index2 += 1L;
			}
			if (this.writeKey != null && this.writeKey.number > this.insertKey.number)
			{
				this.writeKey.index1 += 1L;
				this.writeKey.index2 += 1L;
			}
			if (this.deleteKey != null && this.deleteKey.number > this.insertKey.number)
			{
				this.deleteKey.index1 += 1L;
				this.deleteKey.index2 += 1L;
			}
			this.insertKey.changed = true;
			this.changed = true;
			this.file.isModified = true;
		}
		private void InsertKey(DataKey key, DataKey prevKey)
		{
			for (int i = this.buffer_count; i > prevKey.number + 1; i--)
			{
				this.cache[i] = this.cache[i - 1];
				if (this.cache[i] != null)
				{
					this.cache[i].number = i;
				}
			}
			this.buffer_count++;
			key.number = prevKey.number + 1;
			this.cache[key.number] = key;
			key.prev = prevKey.position;
			key.next = prevKey.next;
			this.WriteKey(key);
			this.file.WriteKey(this.key);
		}
		private void WriteKey(DataKey key)
		{
			long position = key.position;
			this.file.WriteKey(key);
			if (key.position != position)
			{
				DataKey dataKey = null;
				if (key.number != 0)
				{
					dataKey = this.cache[key.number - 1];
				}
				if (dataKey != null)
				{
					dataKey.next = key.position;
					if (!dataKey.changed)
					{
						this.SetNext(key.prev, key.position);
					}
				}
				else
				{
					if (key.prev != -1L)
					{
						this.SetNext(key.prev, key.position);
					}
				}
				DataKey dataKey2 = null;
				if (key.number != this.buffer_count - 1)
				{
					dataKey2 = this.cache[key.number + 1];
				}
				if (dataKey2 != null)
				{
					dataKey2.prev = key.position;
					if (!dataKey2.changed)
					{
						this.SetPrev(key.next, key.position);
					}
				}
				else
				{
					if (key.next != -1L)
					{
						this.SetPrev(key.next, key.position);
					}
				}
			}
			if (key == this.writeKey)
			{
				if (this.position1 == -1L)
				{
					this.position1 = this.writeKey.position;
				}
				this.position2 = this.writeKey.position;
			}
			this.file.WriteKey(this.key);
		}
		private void SetPrev(DataKey key, long position)
		{
			key.prev = position;
			this.SetPrev(key.position, position);
		}
		private void SetNext(DataKey key, long position)
		{
			key.next = position;
			this.SetNext(key.position, position);
		}
		private void SetPrev(long key, long position)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(position);
			this.file.WriteBuffer(memoryStream.GetBuffer(), key + 61L, 8);
		}
		private void SetNext(long key, long position)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(position);
			this.file.WriteBuffer(memoryStream.GetBuffer(), key + 69L, 8);
		}
		private void DeleteKey(DataKey key)
		{
			if (key.position == this.position2)
			{
				if (key.prev != -1L)
				{
					this.position2 = key.prev;
				}
				else
				{
					this.position1 = -1L;
					this.position2 = -1L;
				}
			}
			this.file.DeleteKey(key, false);
			if (key.prev != -1L)
			{
				DataKey dataKey = this.cache[key.number - 1];
				if (dataKey != null)
				{
					dataKey.next = key.next;
					if (!dataKey.changed)
					{
						this.SetNext(key.prev, key.next);
					}
				}
				else
				{
					this.SetNext(key.prev, key.next);
				}
			}
			if (key.next != -1L)
			{
				DataKey dataKey2 = this.cache[key.number + 1];
				if (dataKey2 != null)
				{
					dataKey2.prev = key.prev;
					if (!dataKey2.changed)
					{
						this.SetPrev(key.next, key.prev);
					}
				}
				else
				{
					this.SetPrev(key.next, key.prev);
				}
			}
			for (int i = key.number; i < this.buffer_count - 1; i++)
			{
				this.cache[i] = this.cache[i + 1];
				if (this.cache[i] != null)
				{
					this.cache[i].number = i;
				}
			}
			this.buffer_count--;
			this.file.WriteKey(this.key);
		}
		private DataKey ReadKey(long position)
		{
			byte[] buffer = new byte[79];
			MemoryStream input = new MemoryStream(buffer);
			BinaryReader reader = new BinaryReader(input);
			this.file.ReadBuffer(buffer, position, 77);
			DataKey dataKey = new DataKey(this.file, null, -1L, -1L);
			dataKey.Read(reader, true);
			dataKey.position = position;
			return dataKey;
		}
		private DataKey GetFirstKey()
		{
			DataKey dataKey = this.cache[0];
			if (dataKey == null)
			{
				dataKey = this.ReadKey(this.position1);
				this.cache[0] = dataKey;
			}
			dataKey.number = 0;
			dataKey.index1 = 0L;
			dataKey.index2 = (long)(dataKey.count - 1);
			return dataKey;
		}
		private DataKey GetNextKey(DataKey key)
		{
			if (key.number == -1)
			{
				Console.WriteLine("DataSeries::GetNextKey Error: key.number is not set");
			}
			DataKey dataKey = this.cache[key.number + 1];
			if (dataKey == null)
			{
				if (key.next == -1L)
				{
					Console.WriteLine("DataSeries::GetNextKey Error: key.next is not set");
				}
				dataKey = this.ReadKey(key.next);
				dataKey.number = key.number + 1;
				this.cache[dataKey.number] = dataKey;
			}
			dataKey.index1 = key.index2 + 1L;
			dataKey.index2 = dataKey.index1 + (long)dataKey.count - 1L;
			return dataKey;
		}
		private DataKey GetKey(long index, DataKey key)
		{
			if (index < 0L || index >= this.count)
			{
				Console.WriteLine("DataSeries::GetIndex Error: index is out of range : " + index);
				return null;
			}
			DataKey dataKey = null;
			if (key != null)
			{
				if (index >= key.index1 && index <= key.index2)
				{
					return key;
				}
				if (index > key.index2)
				{
					dataKey = this.GetNextKey(key);
				}
			}
			if (dataKey == null)
			{
				dataKey = this.GetFirstKey();
			}
			while (index < dataKey.index1 || index > dataKey.index2)
			{
				dataKey = this.GetNextKey(dataKey);
			}
			return dataKey;
		}
		private DataKey GetKey(DateTime dateTime, DataKey key)
		{
			if (this.count == 0L || dateTime > this.dateTime2)
			{
				Console.WriteLine("DataSeries::GetKey dateTime is out of range : " + dateTime);
				return null;
			}
			DataKey dataKey = null;
			if (dateTime <= this.dateTime1)
			{
				return this.GetFirstKey();
			}
			if (key != null)
			{
				if (dateTime >= key.dateTime1 && dateTime <= key.dateTime2)
				{
					return key;
				}
				if (dateTime > key.dateTime2)
				{
					dataKey = this.GetNextKey(key);
				}
			}
			if (dataKey == null)
			{
				dataKey = this.GetFirstKey();
			}
			while (!(dateTime >= dataKey.dateTime1) || !(dateTime <= dataKey.dateTime2))
			{
				dataKey = this.GetNextKey(dataKey);
			}
			return dataKey;
		}
		public DataObject Get(long index)
		{
			if (!this.isOpenRead)
			{
				this.OpenRead();
			}
			DataKey dataKey = this.GetKey(index, this.readKey);
			if (dataKey == null)
			{
				return null;
			}
			if (dataKey != this.readKey)
			{
				if (!this.cacheObjects && this.readKey != null && this.readKey != this.writeKey && this.readKey != this.insertKey && this.readKey != this.deleteKey)
				{
					this.readKey.objects = null;
				}
				this.readKey = dataKey;
			}
			return dataKey.GetObjects()[(int)checked((IntPtr)unchecked(index - dataKey.index1))];
		}
		public void Remove(long index)
		{
			if (!this.isOpenWrite)
			{
				this.OpenWrite();
			}
			DataKey dataKey = this.GetKey(index, this.deleteKey);
			if (dataKey == null)
			{
				return;
			}
			if (this.deleteKey == null)
			{
				this.deleteKey = dataKey;
			}
			else
			{
				if (this.deleteKey != dataKey)
				{
					if (this.deleteKey.changed)
					{
						this.WriteKey(this.deleteKey);
					}
					if (!this.cacheObjects && this.deleteKey != this.readKey && this.deleteKey != this.writeKey && this.deleteKey != this.insertKey)
					{
						this.deleteKey.objects = null;
					}
					this.deleteKey = dataKey;
				}
			}
			dataKey.RemoveObject(index - dataKey.index1);
			dataKey.index2 -= 1L;
			if (this.readKey != null && this.readKey.number > dataKey.number)
			{
				this.readKey.index1 -= 1L;
				this.readKey.index2 -= 1L;
			}
			if (this.writeKey != null && this.writeKey.number > dataKey.number)
			{
				this.writeKey.index1 -= 1L;
				this.writeKey.index2 -= 1L;
			}
			if (this.insertKey != null && this.insertKey.number > dataKey.number)
			{
				this.insertKey.index1 -= 1L;
				this.insertKey.index2 -= 1L;
			}
			if (dataKey.count == 0)
			{
				this.DeleteKey(dataKey);
				this.deleteKey = null;
			}
			this.count -= 1L;
			this.changed = true;
			this.file.isModified = true;
		}
		public long GetIndex(DateTime dateTime, SearchOption option = SearchOption.Prev)
		{
			if (!this.isOpenRead)
			{
				this.OpenRead();
			}
			if (this.count == 0L || dateTime > this.dateTime2)
			{
				Console.WriteLine("DataSeries::GetIndex dateTime is out of range : " + dateTime);
				return -1L;
			}
			if (dateTime <= this.dateTime1)
			{
				return 0L;
			}
			DataKey dataKey = this.GetKey(dateTime, this.readKey);
			if (dataKey == null)
			{
				return -1L;
			}
			if (dataKey != this.readKey)
			{
				if (!this.cacheObjects && this.readKey != null && this.readKey != this.writeKey && this.readKey != this.insertKey && this.readKey != this.deleteKey)
				{
					this.readKey.objects = null;
				}
				this.readKey = dataKey;
			}
			return this.readKey.index1 + (long)this.readKey.GetIndex(dateTime, option);
		}
		public DataObject Get(DateTime dateTime)
		{
			if (!this.isOpenRead)
			{
				this.OpenRead();
			}
			if (this.count == 0L || dateTime > this.dateTime2)
			{
				Console.WriteLine("DataSeries::Get dateTime is out of range : " + dateTime);
				return null;
			}
			if (dateTime <= this.dateTime1)
			{
				return this.Get(0L);
			}
			DataKey dataKey = this.GetKey(dateTime, this.readKey);
			if (dataKey == null)
			{
				return null;
			}
			if (dataKey != this.readKey)
			{
				if (!this.cacheObjects && this.readKey != null && this.readKey != this.writeKey && this.readKey != this.insertKey && this.readKey != this.deleteKey)
				{
					this.readKey.objects = null;
				}
				this.readKey = dataKey;
			}
			return this.readKey.GetObject(dateTime);
		}
		public void Clear()
		{
			if (this.cache == null)
			{
				this.ReadCache();
			}
			if (this.position1 != -1L)
			{
				DataKey dataKey = this.ReadKey(this.position1);
				while (true)
				{
					this.file.DeleteKey(dataKey, false);
					if (dataKey.next == -1L)
					{
						break;
					}
					dataKey = this.ReadKey(dataKey.next);
				}
			}
			this.count = 0L;
			this.buffer_count = 0;
			this.dateTime1 = new DateTime(0L);
			this.dateTime2 = new DateTime(0L);
			this.position1 = -1L;
			this.position2 = -1L;
			this.isOpenRead = false;
			this.isOpenWrite = false;
			this.cache = new IdArray<DataKey>(4096);
			this.cacheKey.obj = new DataKeyIdArray(this.cache);
			this.readKey = null;
			this.writeKey = null;
			this.deleteKey = null;
			this.insertKey = null;
			this.changed = true;
			this.Flush();
		}
		internal void Flush()
		{
			if (this.changed)
			{
				if (this.insertKey != null && this.insertKey.changed)
				{
					this.WriteKey(this.insertKey);
				}
				if (this.writeKey != null && this.writeKey.changed)
				{
					this.WriteKey(this.writeKey);
				}
				if (this.deleteKey != null && this.deleteKey.changed)
				{
					this.WriteKey(this.deleteKey);
				}
				this.WriteCache();
				this.file.WriteKey(this.key);
				this.changed = false;
			}
		}
		public void Dump()
		{
			Console.WriteLine("Data series " + this.name);
			Console.WriteLine("Count = " + this.count);
			Console.WriteLine("Position1 = " + this.position1);
			Console.WriteLine("Position2 = " + this.position2);
			Console.WriteLine("DateTime1 = " + this.dateTime1.Ticks);
			Console.WriteLine("DateTime2 = " + this.dateTime2.Ticks);
			Console.WriteLine("Buffer count = " + this.buffer_count);
			Console.WriteLine();
			Console.WriteLine("Keys in cache:");
			Console.WriteLine();
			for (int i = 0; i < this.buffer_count; i++)
			{
				if (this.cache[i] != null)
				{
					Console.WriteLine(this.cache[i]);
				}
			}
			Console.WriteLine();
			Console.WriteLine("Keys on disk:");
			Console.WriteLine();
			if (this.position1 != -1L)
			{
				DataKey dataKey = this.ReadKey(this.position1);
				while (true)
				{
					Console.WriteLine(dataKey);
					if (dataKey.next == -1L)
					{
						break;
					}
					dataKey = this.ReadKey(dataKey.next);
				}
			}
			Console.WriteLine();
			if (this.writeKey != null)
			{
				Console.WriteLine("Write Key : " + this.changed);
			}
			else
			{
				Console.WriteLine("Write Key : null");
			}
			Console.WriteLine();
			Console.WriteLine("End dump");
			Console.WriteLine();
		}
	}
}
