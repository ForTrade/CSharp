using System;
using System.IO;
namespace SmartQuant
{
	public class ProviderErrorStreamer : ObjectStreamer
	{
		public ProviderErrorStreamer()
		{
			this.typeId = 21;
			this.type = typeof(ProviderError);
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			ProviderError providerError = (ProviderError)obj;
			writer.Write(0);
			writer.Write(providerError.dateTime.ToBinary());
			writer.Write((byte)providerError.type);
			writer.Write(providerError.providerId);
			writer.Write(providerError.id);
			writer.Write(providerError.code);
			writer.Write(providerError.text);
		}
		public override object Read(BinaryReader reader)
		{
			ProviderError providerError = new ProviderError();
			reader.ReadByte();
			providerError.dateTime = DateTime.FromBinary(reader.ReadInt64());
			providerError.type = (ProviderErrorType)reader.ReadByte();
			providerError.providerId = reader.ReadByte();
			providerError.id = reader.ReadInt32();
			providerError.code = reader.ReadInt32();
			providerError.text = reader.ReadString();
			return providerError;
		}
	}
}
