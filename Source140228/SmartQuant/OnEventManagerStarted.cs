using System;
namespace SmartQuant
{
	public class OnEventManagerStarted : Event
	{
		public override byte TypeId
		{
			get
			{
				return 207;
			}
		}
	}
}
