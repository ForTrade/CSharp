using System;
using System.Collections;
using System.Collections.Generic;
namespace SmartQuant
{
	public class ProviderList : IEnumerable<IProvider>, IEnumerable
	{
		private Dictionary<int, IProvider> providerById = new Dictionary<int, IProvider>();
		private Dictionary<string, IProvider> providerByName = new Dictionary<string, IProvider>();
		private List<IProvider> providers = new List<IProvider>();
		public int Count
		{
			get
			{
				return this.providers.Count;
			}
		}
		public IProvider this[string name]
		{
			get
			{
				return this.GetByName(name);
			}
		}
		public void Add(IProvider provider)
		{
			this.providers.Add(provider);
			this.providerById[(int)provider.Id] = provider;
			this.providerByName[provider.Name] = provider;
		}
		public void Remove(IProvider provider)
		{
			this.providers.Remove(provider);
			this.providerById.Remove((int)provider.Id);
			this.providerByName.Remove(provider.Name);
		}
		public IProvider GetByName(string name)
		{
			IProvider result;
			this.providerByName.TryGetValue(name, out result);
			return result;
		}
		public IProvider GetById(int id)
		{
			return this.providerById[id];
		}
		public IProvider GetByIndex(int index)
		{
			return this.providers[index];
		}
		public void Clear()
		{
			this.providers.Clear();
			this.providerById.Clear();
			this.providerByName.Clear();
		}
		public IEnumerator<IProvider> GetEnumerator()
		{
			return this.providers.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.providers.GetEnumerator();
		}
	}
}
