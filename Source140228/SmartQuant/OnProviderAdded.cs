using System;
namespace SmartQuant
{
	public class OnProviderAdded : Event
	{
		internal IProvider provider;
		public override byte TypeId
		{
			get
			{
				return 102;
			}
		}
		public OnProviderAdded(IProvider provider)
		{
			this.provider = provider;
		}
	}
}
