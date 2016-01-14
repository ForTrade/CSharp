using System;
namespace SmartQuant
{
	public class OnEventManagerResumed : Event
	{
		public override byte TypeId
		{
			get
			{
				return 210;
			}
		}
	}
}
