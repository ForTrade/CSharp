using System;
namespace SmartQuant
{
	public class OnDisconnect : Event
	{
		public override byte TypeId
		{
			get
			{
				return 202;
			}
		}
	}
}
