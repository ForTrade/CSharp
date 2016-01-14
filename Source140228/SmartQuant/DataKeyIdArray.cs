using System;
namespace SmartQuant
{
	internal class DataKeyIdArray
	{
		internal IdArray<DataKey> keys;
		internal DataKeyIdArray(IdArray<DataKey> keys)
		{
			this.keys = keys;
		}
	}
}
