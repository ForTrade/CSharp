using System;
using System.Collections;
using System.Collections.Generic;
namespace SmartQuant
{
	public class Global : IEnumerable<object>, IEnumerable
	{
		private Dictionary<string, object> objects = new Dictionary<string, object>();
		public int Count
		{
			get
			{
				return this.objects.Count;
			}
		}
		public object this[string key]
		{
			get
			{
				return this.objects[key];
			}
			set
			{
				this.objects[key] = value;
			}
		}
		public bool ContainsKey(string key)
		{
			return this.objects.ContainsKey(key);
		}
		public bool ContainsValue(object obj)
		{
			return this.objects.ContainsValue(obj);
		}
		public void Add(string key, object obj)
		{
			this.objects.Add(key, obj);
		}
		public void Remove(string key)
		{
			this.objects.Remove(key);
		}
		public int GetInt(string key)
		{
			return (int)this.objects[key];
		}
		public double GetDouble(string key)
		{
			return (double)this.objects[key];
		}
		public string GetString(string key)
		{
			return (string)this.objects[key];
		}
		public void Clear()
		{
			this.objects.Clear();
		}
		public IEnumerator<object> GetEnumerator()
		{
			return this.objects.Values.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.objects.Values.GetEnumerator();
		}
	}
}
