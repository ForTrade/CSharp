using System;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace SmartQuant
{
	public struct XmlProvider
	{
		[XmlElement("id")]
		public int ProviderId;
		[XmlElement("instance")]
		public int InstanceId;
		[XmlArray("properties"), XmlArrayItem("property")]
		public List<XmlProviderProperty> Properties;
	}
}
