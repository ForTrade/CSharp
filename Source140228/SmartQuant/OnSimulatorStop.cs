using System;
namespace SmartQuant
{
	public class OnSimulatorStop : DataObject
	{
		public override byte TypeId
		{
			get
			{
				return 108;
			}
		}
		public OnSimulatorStop()
		{
			this.dateTime = DateTime.MinValue;
		}
		public override string ToString()
		{
			return "OnSimulatorStop";
		}
	}
}
