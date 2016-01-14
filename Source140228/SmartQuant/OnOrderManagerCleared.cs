using System;
namespace SmartQuant
{
	public class OnOrderManagerCleared : Event
	{
		public override byte TypeId
		{
			get
			{
				return 122;
			}
		}
	}
}
