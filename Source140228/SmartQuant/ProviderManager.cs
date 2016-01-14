using System;
using System.IO;
using System.Xml.Serialization;
namespace SmartQuant
{
	public class ProviderManager
	{
		private Framework framework;
		private ProviderList providers;
		internal IDataSimulator dataSimulator;
		internal IExecutionSimulator executionSimulator;
		private ProviderManagerSettings settings;
		public ProviderList Providers
		{
			get
			{
				return this.providers;
			}
		}
		public IDataSimulator DataSimulator
		{
			get
			{
				return this.dataSimulator;
			}
			set
			{
				this.dataSimulator = value;
			}
		}
		public IExecutionSimulator ExecutionSimulator
		{
			get
			{
				return this.executionSimulator;
			}
			set
			{
				this.executionSimulator = value;
			}
		}
		public ProviderManager(Framework framework, IDataSimulator dataSimulator = null, IExecutionSimulator executionSimulator = null)
		{
			this.framework = framework;
			this.providers = new ProviderList();
			this.settings = new ProviderManagerSettings();
			this.LoadSettings();
			if (dataSimulator == null)
			{
				this.dataSimulator = new DataSimulator(framework);
			}
			else
			{
				this.dataSimulator = dataSimulator;
			}
			this.AddProvider(this.dataSimulator);
			if (executionSimulator == null)
			{
				this.executionSimulator = new ExecutionSimulator(framework);
			}
			else
			{
				this.executionSimulator = executionSimulator;
			}
			this.AddProvider(this.executionSimulator);
		}
		public void SetDataSimulator(string name)
		{
			this.dataSimulator = (this.GetProvider(name) as IDataSimulator);
		}
		public void SetExecutionSimulator(string name)
		{
			this.executionSimulator = (this.GetProvider(name) as IExecutionSimulator);
		}
		public void SetDataSimulator(int id)
		{
			this.dataSimulator = (this.GetProvider(id) as IDataSimulator);
		}
		public void SetExecutionSimulator(int id)
		{
			this.executionSimulator = (this.GetProvider(id) as IExecutionSimulator);
		}
		public void AddProvider(IProvider provider)
		{
			this.providers.Add(provider);
			this.LoadSettings(provider);
			this.framework.eventServer.OnProviderAdded(provider);
		}
		public void RemoveProvider(Provider provider)
		{
			this.providers.Remove(provider);
			this.framework.eventServer.OnProviderRemoved(provider);
		}
		public IProvider GetProvider(int id)
		{
			return this.providers.GetById(id);
		}
		public IProvider GetProvider(string name)
		{
			return this.providers.GetByName(name);
		}
		public void Clear()
		{
			if (this.dataSimulator != null)
			{
				this.dataSimulator.Clear();
			}
			if (this.executionSimulator != null)
			{
				this.executionSimulator.Clear();
			}
		}
		public void LoadSettings(IProvider provider)
		{
			if (provider is Provider)
			{
				ProviderSettings providerSettings = this.settings.Providers[provider];
				if (providerSettings != null)
				{
					((Provider)provider).SetProperties(providerSettings.Properties);
				}
			}
		}
		public void SaveSettings(IProvider provider)
		{
			ProviderSettings providerSettings = this.settings.Providers[provider];
			if (providerSettings == null)
			{
				providerSettings = new ProviderSettings(provider);
				this.settings.Providers[provider] = providerSettings;
			}
			if (provider is Provider)
			{
				providerSettings.Properties = new ProviderPropertyList(((Provider)provider).GetProperties());
			}
			this.SaveSettings();
		}
		private void LoadSettings()
		{
			try
			{
				string settingsFilePath = this.GetSettingsFilePath();
				if (File.Exists(settingsFilePath))
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlProviderManagerSettings));
					using (FileStream fileStream = new FileStream(settingsFilePath, FileMode.Open))
					{
						XmlProviderManagerSettings xml = (XmlProviderManagerSettings)xmlSerializer.Deserialize(fileStream);
						this.settings.FromXml(xml);
					}
				}
			}
			catch
			{
			}
		}
		private void SaveSettings()
		{
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlProviderManagerSettings));
				using (FileStream fileStream = new FileStream(this.GetSettingsFilePath(), FileMode.Create))
				{
					xmlSerializer.Serialize(fileStream, this.settings.ToXml());
				}
			}
			catch
			{
			}
		}
		private string GetSettingsFilePath()
		{
			return this.framework.configuration.ProviderManagerFileName;
		}
	}
}
