using System;
using System.Collections.Generic;
namespace SmartQuant
{
	internal class ObjectKeyList
	{
		internal Dictionary<string, ObjectKey> keys;
		internal ObjectKeyList(Dictionary<string, ObjectKey> keys)
		{
			this.keys = keys;
		}
	}
}
