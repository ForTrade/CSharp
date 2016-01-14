using System;
namespace SmartQuant
{
	internal class ProviderSettings
	{
		public int ProviderId
		{
			get;
			private set;
		}
		public int InstanceId
		{
			get;
			private set;
		}
		public ProviderPropertyList Properties
		{
			get;
			set;
		}
		public ProviderSettings(int providerId, int instanceId)
		{
			this.ProviderId = providerId;
			this.InstanceId = instanceId;
			this.Properties = new ProviderPropertyList();
		}
		public ProviderSettings(IProvider provider) : this((int)provider.Id, 1)
		{
		}
	}
}
