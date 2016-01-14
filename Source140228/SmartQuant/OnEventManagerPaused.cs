using System;
namespace SmartQuant
{
	public class OnEventManagerPaused : Event
	{
		public override byte TypeId
		{
			get
			{
				return 209;
			}
		}
	}
}
