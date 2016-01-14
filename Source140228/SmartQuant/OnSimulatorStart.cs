using System;
namespace SmartQuant
{
	public class OnSimulatorStart : DataObject
	{
		internal DateTime dateTime1;
		internal DateTime dateTime2;
		internal long count;
		public override byte TypeId
		{
			get
			{
				return 107;
			}
		}
		public OnSimulatorStart()
		{
		}
		public OnSimulatorStart(DateTime dateTime1, DateTime dateTime2, long count)
		{
			this.dateTime1 = dateTime1;
			this.dateTime2 = dateTime2;
			this.count = count;
		}
		public override string ToString()
		{
			return "OnSimulatorStart";
		}
	}
}
