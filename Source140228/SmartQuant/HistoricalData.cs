using System;
namespace SmartQuant
{
	public class HistoricalData : Event
	{
		public override byte TypeId
		{
			get
			{
				return 130;
			}
		}
		public string RequestId
		{
			get;
			set;
		}
		public int TotalNum
		{
			get;
			set;
		}
		public DataObject[] Objects
		{
			get;
			set;
		}
	}
}
