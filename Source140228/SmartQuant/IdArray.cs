using System;
namespace SmartQuant
{
	public class IdArray<T>
	{
		private T[] array;
		private int size;
		private int block_size;
		public int Size
		{
			get
			{
				return this.size;
			}
		}
		public T this[int id]
		{
			get
			{
				if (id >= this.size)
				{
					this.Resize(id);
				}
				return this.array[id];
			}
			set
			{
				if (id >= this.size)
				{
					this.Resize(id);
				}
				this.array[id] = value;
			}
		}
		public IdArray(int size = 1000)
		{
			this.size = size;
			this.block_size = size;
			this.array = new T[size];
			this.Clear();
		}
		public void Clear()
		{
			for (int i = 0; i < this.size; i++)
			{
				this.array[i] = default(T);
			}
		}
		public void Add(int id, T value)
		{
			if (id >= this.size)
			{
				this.Resize(id);
			}
			this.array[id] = value;
		}
		public void Remove(int id)
		{
			if (id >= this.size)
			{
				this.Resize(id);
			}
			this.array[id] = default(T);
		}
		private void Resize(int index)
		{
			Console.WriteLine("IdArray::Resize index = " + index);
			int num = index + this.block_size;
			T[] array = new T[num];
			for (int i = 0; i < this.size; i++)
			{
				array[i] = this.array[i];
			}
			for (int j = this.size; j < num; j++)
			{
				array[j] = default(T);
			}
			this.array = array;
			this.size = num;
		}
	}
}
