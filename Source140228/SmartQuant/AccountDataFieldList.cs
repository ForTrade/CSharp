using System;
using System.Collections;
using System.Collections.Generic;
namespace SmartQuant
{
	public class AccountDataFieldList : ICollection, IEnumerable
	{
		private Dictionary<string, Dictionary<string, object>> table;
		public object this[string name, string currency]
		{
			get
			{
				Dictionary<string, object> dictionary;
				if (!this.table.TryGetValue(name, out dictionary))
				{
					return null;
				}
				object result;
				if (dictionary.TryGetValue(currency, out result))
				{
					return result;
				}
				return null;
			}
			internal set
			{
				Dictionary<string, object> dictionary;
				if (!this.table.TryGetValue(name, out dictionary))
				{
					dictionary = new Dictionary<string, object>();
					this.table.Add(name, dictionary);
				}
				dictionary[currency] = value;
			}
		}
		public object this[string name]
		{
			get
			{
				return this[name, string.Empty];
			}
		}
		public int Count
		{
			get
			{
				int num = 0;
				foreach (Dictionary<string, object> arg_1C_0 in this.table.Values)
				{
					num++;
				}
				return num;
			}
		}
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}
		public object SyncRoot
		{
			get
			{
				return null;
			}
		}
		internal AccountDataFieldList()
		{
			this.table = new Dictionary<string, Dictionary<string, object>>();
		}
		public void Add(string name, string currency, object value)
		{
			Dictionary<string, object> dictionary;
			if (!this.table.TryGetValue(name, out dictionary))
			{
				dictionary = new Dictionary<string, object>();
				this.table.Add(name, dictionary);
			}
			dictionary.Add(currency, value);
		}
		public void Clear()
		{
			this.table.Clear();
		}
		public void Add(string name, object value)
		{
			this.Add(name, string.Empty, value);
		}
		public AccountDataField[] ToArray()
		{
			List<AccountDataField> list = new List<AccountDataField>();
			foreach (KeyValuePair<string, Dictionary<string, object>> current in this.table)
			{
				foreach (KeyValuePair<string, object> current2 in current.Value)
				{
					list.Add(new AccountDataField(current.Key, current2.Key, current2.Value));
				}
			}
			return list.ToArray();
		}
		public void CopyTo(Array array, int index)
		{
			this.ToArray().CopyTo(array, index);
		}
		public IEnumerator GetEnumerator()
		{
			return this.ToArray().GetEnumerator();
		}
	}
}
