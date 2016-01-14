using System;
using System.Collections.Generic;
namespace SmartQuant
{
	internal class ProviderIdByName : Dictionary<string, byte>
	{
		internal ProviderIdByName()
		{
			base.Add("DataSimulator", 1);
			base.Add("ExecutionSimulator", 2);
			base.Add("QuickFIX42", 3);
			base.Add("IB", 4);
			base.Add("ESignal", 5);
			base.Add("MBTrading", 6);
			base.Add("Opentick", 7);
			base.Add("QuoteTracker", 8);
			base.Add("TAL", 9);
			base.Add("TTFIX", 10);
			base.Add("TTAPI", 11);
			base.Add("Genesis", 12);
			base.Add("MyTrack", 13);
			base.Add("Photon", 14);
			base.Add("Bloomberg", 15);
			base.Add("Reuters", 16);
			base.Add("Yahoo", 17);
			base.Add("DC", 18);
			base.Add("CSI", 19);
			base.Add("QuantHouse", 20);
			base.Add("PATSAPI", 21);
			base.Add("OpenECry", 22);
			base.Add("OpenTick", 23);
			base.Add("FIX", 24);
			base.Add("Google", 25);
			base.Add("Hotspot", 26);
			base.Add("AlfaDirect", 27);
			base.Add("Currenex", 28);
			base.Add("SmartCOM", 29);
			base.Add("GenericEOD", 30);
			base.Add("QUIKFIX", 31);
			base.Add("OSLFIX", 32);
			base.Add("Nordnet", 33);
			base.Add("Integral", 35);
			base.Add("QuantRouter", 38);
			base.Add("MatchingEngine", 101);
		}
	}
}
