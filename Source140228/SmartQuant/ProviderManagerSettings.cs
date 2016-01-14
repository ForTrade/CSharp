using System;
using System.Collections.Generic;
namespace SmartQuant
{
	internal class ProviderManagerSettings
	{
		public ProviderSettingsList Providers
		{
			get;
			private set;
		}
		public ProviderManagerSettings()
		{
			this.Providers = new ProviderSettingsList();
		}
		public XmlProviderManagerSettings ToXml()
		{
			XmlProviderManagerSettings result = default(XmlProviderManagerSettings);
			result.Providers = new List<XmlProvider>();
			foreach (ProviderSettings providerSettings in this.Providers)
			{
				XmlProvider item = default(XmlProvider);
				item.ProviderId = providerSettings.ProviderId;
				item.InstanceId = providerSettings.InstanceId;
				item.Properties = new List<XmlProviderProperty>();
				foreach (ProviderProperty current in providerSettings.Properties.All)
				{
					XmlProviderProperty item2 = default(XmlProviderProperty);
					item2.Name = current.Name;
					item2.Value = current.Value;
					item.Properties.Add(item2);
				}
				result.Providers.Add(item);
			}
			return result;
		}
		public void FromXml(XmlProviderManagerSettings xml)
		{
			this.Providers.Clear();
			if (xml.Providers != null)
			{
				foreach (XmlProvider current in xml.Providers)
				{
					ProviderSettings providerSettings = new ProviderSettings(current.ProviderId, current.InstanceId);
					if (current.Properties != null)
					{
						foreach (XmlProviderProperty current2 in current.Properties)
						{
							providerSettings.Properties.SetValue(current2.Name, current2.Value);
						}
					}
					this.Providers[new ProviderSettingsKey(providerSettings.ProviderId, providerSettings.InstanceId)] = providerSettings;
				}
			}
		}
	}
}
