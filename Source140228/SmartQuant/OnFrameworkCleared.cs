using System;
namespace SmartQuant
{
	public class OnFrameworkCleared : Event
	{
		internal Framework framework;
		public override byte TypeId
		{
			get
			{
				return 99;
			}
		}
		public OnFrameworkCleared(Framework framework)
		{
			this.framework = framework;
		}
	}
}
