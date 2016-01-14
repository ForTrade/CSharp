using System;
using System.Collections.Generic;
namespace SmartQuant
{
	internal class BarFieldByName : Dictionary<string, byte>
	{
		internal BarFieldByName()
		{
			base.Add("Close", 0);
			base.Add("Open", 1);
			base.Add("High", 2);
			base.Add("Low", 3);
			base.Add("Median", 4);
			base.Add("Typical", 5);
			base.Add("Weighted", 6);
			base.Add("Volume", 8);
			base.Add("OpenInt", 9);
			base.Add("Range", 10);
			base.Add("Mean", 11);
			base.Add("Variance", 12);
			base.Add("StdDev", 13);
		}
	}
}
