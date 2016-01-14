using System;
using System.Collections.Generic;
namespace SmartQuant
{
	internal class AccountDataKey
	{
		private string key;
		public AccountDataKey(AccountData data, params string[] fieldNames)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < fieldNames.Length; i++)
			{
				string fieldName = fieldNames[i];
				list.Add(this.GetValue(data, fieldName));
			}
			this.key = string.Join("\u0001", list);
		}
		private string GetValue(AccountData data, string fieldName)
		{
			object obj = data.Fields[fieldName];
			if (obj != null)
			{
				return obj.ToString();
			}
			return string.Empty;
		}
		public override int GetHashCode()
		{
			return this.key.GetHashCode();
		}
		public override bool Equals(object obj)
		{
			if (obj is AccountDataKey)
			{
				return this.key.Equals(((AccountDataKey)obj).key);
			}
			return base.Equals(obj);
		}
		public override string ToString()
		{
			return this.key;
		}
	}
}
