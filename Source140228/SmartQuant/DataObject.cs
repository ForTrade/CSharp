using System;
namespace SmartQuant
{
	public class DataObject : Event
	{
		public override byte TypeId
		{
			get
			{
				return 0;
			}
		}
		public DataObject()
		{
		}
		public DataObject(DateTime dateTime)
		{
			this.dateTime = dateTime;
		}
		public DataObject(DataObject obj)
		{
			this.dateTime = obj.dateTime;
		}
	}
}
