using System;
using System.Collections;
using System.Collections.Generic;
namespace SmartQuant
{
	internal class ProviderSettingsList : IEnumerable
	{
		private Dictionary<ProviderSettingsKey, ProviderSettings> table;
		public ProviderSettings this[ProviderSettingsKey key]
		{
			get
			{
				ProviderSettings result;
				if (!this.table.TryGetValue(key, out result))
				{
					return null;
				}
				return result;
			}
			set
			{
				this.table[key] = value;
			}
		}
		public ProviderSettings this[IProvider provider]
		{
			get
			{
				return this[new ProviderSettingsKey(provider)];
			}
			set
			{
				this[new ProviderSettingsKey(provider)] = value;
			}
		}
		public ProviderSettingsList()
		{
			this.table = new Dictionary<ProviderSettingsKey, ProviderSettings>();
		}
		public void Clear()
		{
			this.table.Clear();
		}
		public IEnumerator GetEnumerator()
		{
			return this.table.Values.GetEnumerator();
		}
	}
}
