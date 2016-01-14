using System;
namespace SmartQuant
{
	public class OnProviderRemoved : Event
	{
		internal Provider provider;
		public override byte TypeId
		{
			get
			{
				return 103;
			}
		}
		public OnProviderRemoved(Provider provider)
		{
			this.provider = provider;
		}
	}
}
