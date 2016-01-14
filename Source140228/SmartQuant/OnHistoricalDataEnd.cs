using System;
namespace SmartQuant
{
	public class OnHistoricalDataEnd : Event
	{
		internal HistoricalDataEnd end;
		public override byte TypeId
		{
			get
			{
				return 126;
			}
		}
		internal OnHistoricalDataEnd(HistoricalDataEnd end)
		{
			this.end = end;
		}
	}
}
