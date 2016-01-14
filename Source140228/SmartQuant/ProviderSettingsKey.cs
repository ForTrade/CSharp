using System;
namespace SmartQuant
{
	internal class ProviderSettingsKey
	{
		private string key;
		public ProviderSettingsKey(int providerId, int instanceId)
		{
			this.key = string.Format("{0}:{1}", providerId, instanceId);
		}
		public ProviderSettingsKey(IProvider provider) : this((int)provider.Id, 1)
		{
		}
		public override int GetHashCode()
		{
			return this.key.GetHashCode();
		}
		public override bool Equals(object obj)
		{
			if (obj is ProviderSettingsKey)
			{
				return this.key.Equals(((ProviderSettingsKey)obj).key);
			}
			return base.Equals(obj);
		}
		public override string ToString()
		{
			return this.key;
		}
	}
}
