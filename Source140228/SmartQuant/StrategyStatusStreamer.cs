using System;
using System.IO;
namespace SmartQuant
{
	public class StrategyStatusStreamer : ObjectStreamer
	{
		public StrategyStatusStreamer()
		{
			this.typeId = 20;
			this.type = typeof(StrategyStatusInfo);
		}
		public override object Read(BinaryReader reader)
		{
			reader.ReadByte();
			DateTime dateTime = new DateTime(reader.ReadInt64());
			StrategyStatusType type = (StrategyStatusType)reader.ReadByte();
			return new StrategyStatusInfo(dateTime, type)
			{
				Solution = reader.ReadString(),
				Mode = reader.ReadString()
			};
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			byte value = 0;
			writer.Write(value);
			StrategyStatusInfo strategyStatusInfo = obj as StrategyStatusInfo;
			writer.Write(strategyStatusInfo.DateTime.Ticks);
			writer.Write((byte)strategyStatusInfo.Type);
			writer.Write(strategyStatusInfo.Solution);
			writer.Write(strategyStatusInfo.Mode);
		}
	}
}
