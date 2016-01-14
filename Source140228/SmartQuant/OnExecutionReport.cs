using System;
namespace SmartQuant
{
	public class OnExecutionReport : Event
	{
		internal ExecutionReport report;
		public override byte TypeId
		{
			get
			{
				return 115;
			}
		}
		public OnExecutionReport(ExecutionReport report)
		{
			this.report = report;
		}
	}
}
