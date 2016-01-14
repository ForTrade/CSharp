using System;
namespace SmartQuant
{
	public class OnSimulatorProgress : DataObject
	{
		internal long count;
		internal int percent;
		public override byte TypeId
		{
			get
			{
				return 109;
			}
		}
		public OnSimulatorProgress()
		{
			this.dateTime = DateTime.MinValue;
		}
		public OnSimulatorProgress(long count, int percent)
		{
			this.count = count;
			this.percent = percent;
		}
		public override string ToString()
		{
			return "OnSimulatorProgress";
		}
	}
}
