using System;
using System.Collections.Generic;
namespace SmartQuant
{
	public class PermanentQueue<T>
	{
		private List<T> list = new List<T>();
		private Dictionary<object, int> readerTable = new Dictionary<object, int>();
		public void Enqueue(T item)
		{
			lock (this.list)
			{
				this.list.Add(item);
			}
		}
		public T[] DequeueAll(object reader)
		{
			T[] result;
			lock (this.list)
			{
				int num = this.readerTable[reader];
				if (this.list.Count < num + 1)
				{
					result = null;
				}
				else
				{
					T[] array = new T[this.list.Count - num];
					this.list.CopyTo(num, array, 0, array.Length);
					this.readerTable[reader] = num + array.Length;
					result = array;
				}
			}
			return result;
		}
		public void AddReader(object reader)
		{
			lock (this.list)
			{
				this.readerTable[reader] = 0;
			}
		}
		public void RemoveReader(object reader)
		{
			lock (this.list)
			{
				this.readerTable.Remove(reader);
			}
		}
		public int Count(int startIndex)
		{
			int result;
			lock (this.list)
			{
				result = this.list.Count - startIndex;
			}
			return result;
		}
		public void Clear()
		{
			lock (this.list)
			{
				foreach (object current in new List<object>(this.readerTable.Keys))
				{
					this.readerTable[current] = 0;
				}
				this.list.Clear();
			}
		}
	}
}
