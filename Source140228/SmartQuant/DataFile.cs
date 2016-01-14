using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
namespace SmartQuant
{
	public class DataFile
	{
		internal const int headerLength = 62;
		internal string label = "SmartQuant";
		internal byte version = 1;
		internal long beginPosition;
		internal long endPosition;
		internal long keysPosition;
		internal long freePosition;
		internal int keysLength;
		internal int freeLength;
		internal int keysNumber;
		internal int freeNumber;
		internal byte compressionMethod = 1;
		internal byte compressionLevel = 1;
		internal string name;
		internal Stream stream;
		internal FileMode mode;
		internal bool isOpen;
		internal bool isModified;
		internal Dictionary<string, ObjectKey> keys = new Dictionary<string, ObjectKey>();
		internal List<FreeKey> free = new List<FreeKey>();
		internal ObjectKey keysListKey;
		internal ObjectKey freeListKey;
		internal StreamerManager streamerManager;
		private bool disposed;
		private MemoryStream stream_;
		private BinaryWriter writer_;
		public Dictionary<string, ObjectKey> Keys
		{
			get
			{
				return this.keys;
			}
		}
		public byte CompressionMethod
		{
			get
			{
				return this.compressionMethod;
			}
			set
			{
				this.compressionMethod = value;
			}
		}
		public byte CompressionLevel
		{
			get
			{
				return this.compressionLevel;
			}
			set
			{
				this.compressionLevel = value;
			}
		}
		public DataFile(string name, StreamerManager streamerManager)
		{
			this.name = name;
			this.streamerManager = streamerManager;
			this.stream_ = new MemoryStream();
			this.writer_ = new BinaryWriter(this.stream_);
		}
		protected virtual bool OpenFileStream(string name, FileMode mode)
		{
			this.stream = new FileStream(name, mode);
			return this.stream.Length != 0L;
		}
		protected virtual void CloseFileStream()
		{
			this.stream.Close();
		}
		public virtual void Open(FileMode mode = FileMode.OpenOrCreate)
		{
			if (mode != FileMode.OpenOrCreate && mode != FileMode.Create)
			{
				Console.WriteLine("DataFile::Open Can not open file in " + mode + " mode. DataFile suppports FileMode.OpenOrCreate and FileMode.Create modes.");
				return;
			}
			if (this.isOpen)
			{
				Console.WriteLine("DataFile::Open File is already open: " + this.name);
				return;
			}
			this.mode = mode;
			if (!this.OpenFileStream(this.name, mode))
			{
				this.beginPosition = 62L;
				this.endPosition = 62L;
				this.keysPosition = 62L;
				this.freePosition = 62L;
				this.keysLength = 0;
				this.freeLength = 0;
				this.keysNumber = 0;
				this.freeNumber = 0;
				this.isModified = true;
				this.WriteHeader();
			}
			else
			{
				if (!this.ReadHeader())
				{
					Console.WriteLine("DataFile::Open Error opening file " + this.name);
					return;
				}
				this.ReadKeys();
				this.ReadFree();
			}
			this.isOpen = true;
		}
		internal bool ReadHeader()
		{
			byte[] buffer = new byte[62];
			this.ReadBuffer(buffer, 0L, 62);
			MemoryStream input = new MemoryStream(buffer);
			BinaryReader binaryReader = new BinaryReader(input);
			this.label = binaryReader.ReadString();
			if (this.label != "SmartQuant")
			{
				Console.WriteLine("DataFile::ReadHeader This is not SmartQuant data file!");
				return false;
			}
			this.version = binaryReader.ReadByte();
			this.beginPosition = binaryReader.ReadInt64();
			this.endPosition = binaryReader.ReadInt64();
			this.keysPosition = binaryReader.ReadInt64();
			this.freePosition = binaryReader.ReadInt64();
			this.keysLength = binaryReader.ReadInt32();
			this.freeLength = binaryReader.ReadInt32();
			this.keysNumber = binaryReader.ReadInt32();
			this.freeNumber = binaryReader.ReadInt32();
			this.compressionMethod = binaryReader.ReadByte();
			this.compressionLevel = binaryReader.ReadByte();
			return true;
		}
		internal void WriteHeader()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(this.label);
			binaryWriter.Write(this.version);
			binaryWriter.Write(this.beginPosition);
			binaryWriter.Write(this.endPosition);
			binaryWriter.Write(this.keysPosition);
			binaryWriter.Write(this.freePosition);
			binaryWriter.Write(this.keysLength);
			binaryWriter.Write(this.freeLength);
			binaryWriter.Write(this.keysNumber);
			binaryWriter.Write(this.freeNumber);
			binaryWriter.Write(this.compressionMethod);
			binaryWriter.Write(this.compressionLevel);
			this.WriteBuffer(memoryStream.GetBuffer(), 0L, (int)memoryStream.Length);
		}
		protected internal virtual void ReadBuffer(byte[] buffer, long position, int length)
		{
			bool flag = false;
			try
			{
				Monitor.Enter(this, ref flag);
				this.stream.Seek(position, SeekOrigin.Begin);
				this.stream.Read(buffer, 0, length);
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}
		protected internal virtual void WriteBuffer(byte[] buffer, long position, int length)
		{
			bool flag = false;
			try
			{
				Monitor.Enter(this, ref flag);
				this.stream.Seek(position, SeekOrigin.Begin);
				this.stream.Write(buffer, 0, length);
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}
		public void Write(string name, object obj)
		{
			ObjectKey objectKey;
			this.keys.TryGetValue(name, out objectKey);
			if (objectKey != null)
			{
				objectKey.obj = obj;
				objectKey.Init(this);
			}
			else
			{
				objectKey = new ObjectKey(this, name, obj);
				this.keys.Add(name, objectKey);
				this.keysNumber++;
			}
			objectKey.dateTime = DateTime.Now;
			if (objectKey.typeId == 101)
			{
				((DataSeries)obj).Init(this, objectKey);
			}
			this.WriteKey(objectKey);
		}
		public object Get(string name)
		{
			ObjectKey objectKey;
			this.keys.TryGetValue(name, out objectKey);
			if (objectKey != null)
			{
				return objectKey.GetObject();
			}
			return null;
		}
		public void Delete(string name)
		{
			ObjectKey objectKey;
			this.keys.TryGetValue(name, out objectKey);
			if (objectKey != null)
			{
				this.DeleteKey(objectKey, true);
			}
		}
		private FreeKey GetFree(int length)
		{
			foreach (FreeKey current in this.free)
			{
				if (current.length >= length)
				{
					return current;
				}
			}
			return null;
		}
		internal void DeleteKey(ObjectKey key, bool remove = true)
		{
			key.deleted = true;
			this.WriteBuffer(new byte[]
			{
				1
			}, key.position + 5L, 1);
			if (remove)
			{
				this.keys.Remove(key.name);
				this.keysNumber--;
			}
			this.free.Add(new FreeKey(key));
			this.free.Sort();
			this.freeNumber++;
			this.isModified = true;
		}
		internal ObjectKey ReadKey(long position, int length)
		{
			byte[] buffer = new byte[this.keysLength];
			this.ReadBuffer(buffer, position, length);
			MemoryStream input = new MemoryStream(buffer);
			BinaryReader reader = new BinaryReader(input);
			ObjectKey objectKey = new ObjectKey(this, null, null);
			objectKey.Read(reader, true);
			objectKey.position = position;
			return objectKey;
		}
		internal void WriteKey(ObjectKey key)
		{
			this.stream_.SetLength(0L);
			key.Write(this.writer_);
			if (key.position != -1L)
			{
				if (this.stream_.Length > (long)key.recLength)
				{
					this.DeleteKey(key, false);
					key.recLength = (int)this.stream_.Length;
					FreeKey freeKey;
					if (key == this.freeListKey)
					{
						freeKey = this.GetFree(key.keyLength + key.objLength - 17);
					}
					else
					{
						freeKey = this.GetFree(key.keyLength + key.objLength);
					}
					if (freeKey != null)
					{
						key.position = freeKey.position;
						key.recLength = freeKey.length;
						this.free.Remove(freeKey);
						this.freeNumber--;
						if (key == this.freeListKey)
						{
							this.stream_.SetLength(0L);
							key.Write(this.writer_);
						}
					}
					else
					{
						key.position = this.endPosition;
						this.endPosition += (long)key.recLength;
					}
				}
			}
			else
			{
				key.position = this.endPosition;
				this.endPosition += (long)key.recLength;
			}
			this.WriteBuffer(this.stream_.GetBuffer(), key.position, (int)this.stream_.Length);
			key.changed = false;
			this.isModified = true;
		}
		protected void ReadKeys()
		{
			if (this.keysLength == 0)
			{
				return;
			}
			byte[] buffer = new byte[this.keysLength];
			this.ReadBuffer(buffer, this.keysPosition, this.keysLength);
			MemoryStream input = new MemoryStream(buffer);
			BinaryReader reader = new BinaryReader(input);
			ObjectKey objectKey = new ObjectKey(this, null, null);
			objectKey.Read(reader, true);
			objectKey.position = this.keysPosition;
			this.keys = ((ObjectKeyList)objectKey.GetObject()).keys;
			foreach (ObjectKey current in this.keys.Values)
			{
				current.Init(this);
			}
			this.keysListKey = objectKey;
		}
		protected void ReadFree()
		{
			if (this.freeLength == 0)
			{
				return;
			}
			byte[] buffer = new byte[this.freeLength];
			this.ReadBuffer(buffer, this.freePosition, this.freeLength);
			MemoryStream input = new MemoryStream(buffer);
			BinaryReader reader = new BinaryReader(input);
			ObjectKey objectKey = new ObjectKey(this, null, null);
			objectKey.Read(reader, true);
			objectKey.position = this.freePosition;
			this.free = ((FreeKeyList)objectKey.GetObject()).keys;
			this.freeListKey = objectKey;
		}
		private void WriteKeys()
		{
			if (this.keysListKey != null)
			{
				this.DeleteKey(this.keysListKey, false);
			}
			this.keysListKey = new ObjectKey(this, "ObjectKeys", new ObjectKeyList(this.keys));
			this.keysListKey.compressionLevel = 0;
			this.WriteKey(this.keysListKey);
			this.keysPosition = this.keysListKey.position;
			this.keysLength = this.keysListKey.keyLength + this.keysListKey.objLength;
		}
		private void WriteFree()
		{
			if (this.freeListKey != null)
			{
				this.DeleteKey(this.freeListKey, false);
			}
			this.freeListKey = new ObjectKey(this, "FreeKeys", new FreeKeyList(this.free));
			this.freeListKey.compressionLevel = 0;
			this.WriteKey(this.freeListKey);
			this.freePosition = this.freeListKey.position;
			this.freeLength = this.freeListKey.keyLength + this.freeListKey.objLength;
		}
		public void Dump()
		{
			if (this.isOpen)
			{
				Console.WriteLine(string.Concat(new object[]
				{
					"DataFile ",
					this.name,
					" is open in ",
					this.mode,
					" mode and contains ",
					this.keys.Values.Count,
					" objects:"
				}));
				foreach (ObjectKey current in this.keys.Values)
				{
					current.Dump();
				}
				Console.WriteLine("Free objects = " + this.freeNumber);
				using (List<FreeKey>.Enumerator enumerator2 = this.free.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						FreeKey current2 = enumerator2.Current;
						Console.WriteLine(current2.position + " " + current2.length);
					}
					return;
				}
			}
			Console.WriteLine("ObjectFile " + this.name + " is closed");
		}
		public virtual void Flush()
		{
			if (this.isModified)
			{
				foreach (ObjectKey current in this.keys.Values)
				{
					if (current.typeId == 101 && current.obj != null)
					{
						DataSeries dataSeries = (DataSeries)current.obj;
						if (dataSeries.changed)
						{
							dataSeries.Flush();
						}
					}
				}
				this.WriteKeys();
				this.WriteFree();
				this.WriteHeader();
			}
			this.isModified = false;
		}
		public virtual void Close()
		{
			if (!this.isOpen)
			{
				Console.WriteLine("DataFile::Close File is not open: " + this.name);
				return;
			}
			this.Flush();
			this.CloseFileStream();
			this.isOpen = false;
		}
		public void Dispose()
		{
			Console.WriteLine("DataFile::Dispose");
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.Close();
				}
				this.disposed = true;
			}
		}
		~DataFile()
		{
			this.Dispose(false);
		}
	}
}
