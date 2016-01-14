using System;
using System.Xml.Serialization;
namespace SmartQuant
{
	[XmlRoot("Configuration")]
	public class Configuration
	{
		[XmlElement("IsInstrumentFileLocal")]
		public bool IsInstrumentFileLocal;
		[XmlElement("InstrumentFileHost")]
		public string InstrumentFileHost;
		[XmlElement("InstrumentFileName")]
		public string InstrumentFileName;
		[XmlElement("IsDataFileLocal")]
		public bool IsDataFileLocal;
		[XmlElement("DataFileHost")]
		public string DataFileHost;
		[XmlElement("DataFileName")]
		public string DataFileName;
		[XmlElement("DefaultCurrency")]
		public string DefaultCurrency;
		[XmlElement("DefaultExchange")]
		public string DefaultExchange;
		[XmlElement("DefaultDataProvider")]
		public string DefaultDataProvider;
		[XmlElement("DefaultExecutionProvider")]
		public string DefaultExecutionProvider;
		[XmlElement("ProviderManagerFileName")]
		public string ProviderManagerFileName;
		public Configuration()
		{
			this.IsInstrumentFileLocal = true;
			this.InstrumentFileHost = "127.0.0.1";
			this.InstrumentFileName = Installation.DataDir.FullName + "\\instruments.quant";
			this.IsDataFileLocal = true;
			this.DataFileHost = "127.0.0.1";
			this.DataFileName = Installation.DataDir.FullName + "\\data.quant";
			this.DefaultCurrency = "USD";
			this.DefaultExchange = "SMART";
			this.DefaultDataProvider = "QuantRouter";
			this.DefaultExecutionProvider = "QuantRouter";
			this.ProviderManagerFileName = Installation.ConfigDir.FullName + "\\providermanager.xml";
		}
	}
}
