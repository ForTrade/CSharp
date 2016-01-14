using System;
using System.IO;
namespace SmartQuant
{
	public class ExecutionCommandStreamer : ObjectStreamer
	{
		public override object Read(BinaryReader reader)
		{
			reader.ReadByte();
			return new ExecutionCommand
			{
				id = reader.ReadInt32(),
				providerId = reader.ReadInt16(),
				portfolioId = reader.ReadInt16(),
				transactTime = new DateTime(reader.ReadInt64()),
				type = (ExecutionCommandType)reader.ReadByte(),
				instrumentId = reader.ReadInt32(),
				side = (OrderSide)reader.ReadInt32(),
				orderType = (OrderType)reader.ReadInt32(),
				timeInForce = (TimeInForce)reader.ReadInt32(),
				price = reader.ReadDouble(),
				stopPx = reader.ReadDouble(),
				qty = reader.ReadDouble(),
				oCA = reader.ReadString(),
				text = reader.ReadString()
			};
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			byte value = 0;
			writer.Write(value);
			ExecutionCommand executionCommand = obj as ExecutionCommand;
			writer.Write(executionCommand.id);
			writer.Write(executionCommand.providerId);
			writer.Write(executionCommand.portfolioId);
			writer.Write(executionCommand.transactTime.Ticks);
			writer.Write((byte)executionCommand.Type);
			writer.Write(executionCommand.instrumentId);
			writer.Write((int)executionCommand.Side);
			writer.Write((int)executionCommand.orderType);
			writer.Write((int)executionCommand.timeInForce);
			writer.Write(executionCommand.Price);
			writer.Write(executionCommand.StopPx);
			writer.Write(executionCommand.Qty);
			writer.Write(executionCommand.OCA);
			writer.Write(executionCommand.Text);
		}
	}
}
