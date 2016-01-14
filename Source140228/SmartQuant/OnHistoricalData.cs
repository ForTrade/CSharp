using System;
namespace SmartQuant
{
	public class OnHistoricalData : Event
	{
		internal HistoricalData data;
		public override byte TypeId
		{
			get
			{
				return 125;
			}
		}
		internal OnHistoricalData(HistoricalData data)
		{
			this.data = data;
		}
	}
}
