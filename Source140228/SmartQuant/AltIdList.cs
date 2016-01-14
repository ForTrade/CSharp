using System;
using System.Collections;
using System.Collections.Generic;
namespace SmartQuant
{
	public class AltIdList : IEnumerable<AltId>, IEnumerable
	{
		private IdArray<AltId> idByProvider = new IdArray<AltId>(1000);
		private List<AltId> ids = new List<AltId>();
		public int Count
		{
			get
			{
				return this.ids.Count;
			}
		}
		public AltId this[int i]
		{
			get
			{
				return this.ids[i];
			}
		}
		public void Clear()
		{
			this.idByProvider.Clear();
			this.ids.Clear();
		}
		public void Add(AltId id)
		{
			this.idByProvider[(int)id.providerId] = id;
			this.ids.Add(id);
		}
		public void Remove(AltId id)
		{
			this.idByProvider.Remove((int)id.providerId);
			this.ids.Remove(id);
		}
		public AltId Get(byte providerId)
		{
			return this.idByProvider[(int)providerId];
		}
		public IEnumerator<AltId> GetEnumerator()
		{
			return this.ids.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.ids.GetEnumerator();
		}
	}
}
