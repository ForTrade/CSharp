using System;
using System.Collections.Generic;
namespace SmartQuant
{
	public class Transaction
	{
		private List<Fill> fills = new List<Fill>();
		public void Add(Fill fill)
		{
			this.fills.Add(fill);
		}
	}
}
