using System;
using System.Collections.Generic;
namespace SmartQuant
{
	public class ProviderPropertyList
	{
		private Dictionary<string, string> dictionary;
		internal IEnumerable<ProviderProperty> All
		{
			get
			{
				foreach (KeyValuePair<string, string> current in this.dictionary)
				{
					KeyValuePair<string, string> keyValuePair = current;
					string arg_72_0 = keyValuePair.Key;
					KeyValuePair<string, string> keyValuePair2 = current;
					yield return new ProviderProperty(arg_72_0, keyValuePair2.Value);
				}
				yield break;
			}
		}
		public ProviderPropertyList()
		{
			this.dictionary = new Dictionary<string, string>();
		}
		internal ProviderPropertyList(ProviderPropertyList list)
		{
			this.dictionary = new Dictionary<string, string>(list.dictionary);
		}
		public void SetValue(string name, string value)
		{
			this.dictionary[name] = value;
		}
		public string GetStringValue(string name, string defaultValue)
		{
			string result;
			if (!this.dictionary.TryGetValue(name, out result))
			{
				return defaultValue;
			}
			return result;
		}
	}
}
