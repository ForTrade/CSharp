using System;
namespace SmartQuant
{
	internal class ProviderProperty
	{
		public string Name
		{
			get;
			private set;
		}
		public string Value
		{
			get;
			private set;
		}
		public ProviderProperty(string name, string value)
		{
			this.Name = name;
			this.Value = value;
		}
	}
}
