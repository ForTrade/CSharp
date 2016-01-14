using System;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace SmartQuant
{
	[XmlRoot("settings")]
	public struct XmlProviderManagerSettings
	{
		[XmlArray("providers"), XmlArrayItem("provider")]
		public List<XmlProvider> Providers;
	}
}
