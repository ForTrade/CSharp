using System;
using System.Collections.Generic;
namespace SmartQuant
{
	internal class IdName
	{
		private Dictionary<string, byte> idByName = new Dictionary<string, byte>();
		private IdArray<string> nameById = new IdArray<string>(1000);
		internal IdName()
		{
			this.Add("USD", 1);
			this.Add("EUR", 2);
			this.Add("RUR", 3);
		}
		internal void Add(string name, byte id)
		{
			this.idByName.Add(name, id);
			this.nameById.Add((int)id, name);
		}
		internal string GetName(byte id)
		{
			string text = this.nameById[(int)id];
			if (text != null)
			{
				return text;
			}
			return id.ToString();
		}
		internal byte GetId(string name)
		{
			byte result;
			if (!this.idByName.TryGetValue(name, out result))
			{
				return 0;
			}
			return result;
		}
	}
}
