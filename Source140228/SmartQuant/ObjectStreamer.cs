using System;
using System.IO;
namespace SmartQuant
{
	public class ObjectStreamer
	{
		protected internal byte typeId;
		protected internal Type type;
		protected internal StreamerManager streamerManager;
		public StreamerManager StreamerManager
		{
			get
			{
				return this.streamerManager;
			}
		}
		public ObjectStreamer()
		{
			this.typeId = 0;
			this.type = typeof(object);
		}
		public virtual object Read(BinaryReader reader)
		{
			reader.ReadByte();
			return new object();
		}
		public virtual void Write(BinaryWriter writer, object obj)
		{
			byte value = 0;
			writer.Write(value);
		}
	}
}
