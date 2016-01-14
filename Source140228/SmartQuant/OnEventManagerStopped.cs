using System;
namespace SmartQuant
{
	public class OnEventManagerStopped : Event
	{
		public override byte TypeId
		{
			get
			{
				return 208;
			}
		}
	}
}
