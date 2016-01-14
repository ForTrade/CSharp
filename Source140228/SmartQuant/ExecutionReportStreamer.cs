using System;
using System.IO;
namespace SmartQuant
{
	public class ExecutionReportStreamer : ObjectStreamer
	{
		public override object Read(BinaryReader reader)
		{
			reader.ReadByte();
			return new ExecutionReport
			{
				DateTime = new DateTime(reader.ReadInt64()),
				instrumentId = reader.ReadInt32(),
				commandID = reader.ReadInt32(),
				currencyId = reader.ReadByte(),
				execType = (ExecType)reader.ReadInt32(),
				ordType = (OrderType)reader.ReadInt32(),
				side = (OrderSide)reader.ReadInt32(),
				timeInForce = (TimeInForce)reader.ReadInt32(),
				ordStatus = (OrderStatus)reader.ReadInt32(),
				lastPx = reader.ReadDouble(),
				avgPx = reader.ReadDouble(),
				ordQty = reader.ReadDouble(),
				cumQty = reader.ReadDouble(),
				lastQty = reader.ReadDouble(),
				leavesQty = reader.ReadDouble(),
				price = reader.ReadDouble(),
				stopPx = reader.ReadDouble(),
				commission = reader.ReadDouble()
			};
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			byte value = 0;
			writer.Write(value);
			ExecutionReport executionReport = obj as ExecutionReport;
			writer.Write(executionReport.dateTime.Ticks);
			writer.Write(executionReport.instrument.Id);
			writer.Write(executionReport.commandID);
			writer.Write((int)executionReport.currencyId);
			writer.Write((int)executionReport.execType);
			writer.Write((int)executionReport.ordType);
			writer.Write((int)executionReport.side);
			writer.Write((int)executionReport.timeInForce);
			writer.Write((int)executionReport.ordStatus);
			writer.Write(executionReport.lastPx);
			writer.Write(executionReport.avgPx);
			writer.Write(executionReport.ordQty);
			writer.Write(executionReport.cumQty);
			writer.Write(executionReport.lastQty);
			writer.Write(executionReport.leavesQty);
			writer.Write(executionReport.price);
			writer.Write(executionReport.stopPx);
			writer.Write(executionReport.commission);
		}
	}
}
