using System;
using System.Xml.Serialization;
namespace SmartQuant
{
	public struct XmlProviderProperty
	{
		[XmlAttribute("name")]
		public string Name;
		[XmlText]
		public string Value;
	}
}
