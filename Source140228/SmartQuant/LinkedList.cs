using System;
namespace SmartQuant
{
	public class LinkedList<T>
	{
		public LinkedListNode<T> First;
		public int Count;
		public void Add(T data)
		{
			if (this.First == null)
			{
				this.First = new LinkedListNode<T>(data);
				this.Count++;
				return;
			}
			LinkedListNode<T> linkedListNode = this.First;
			while (linkedListNode.Next != null)
			{
				if (linkedListNode.Data.Equals(data))
				{
					return;
				}
				linkedListNode = linkedListNode.Next;
			}
			if (linkedListNode.Data.Equals(data))
			{
				return;
			}
			linkedListNode.Next = new LinkedListNode<T>(data);
			this.Count++;
		}
		public void Remove(T data)
		{
			if (this.First == null)
			{
				return;
			}
			if (this.First.Data.Equals(data))
			{
				this.First = this.First.Next;
				this.Count--;
				return;
			}
			LinkedListNode<T> linkedListNode = this.First;
			for (LinkedListNode<T> next = this.First.Next; next != null; next = next.Next)
			{
				if (next.Data.Equals(data))
				{
					linkedListNode.Next = next.Next;
					this.Count--;
					return;
				}
				linkedListNode = next;
			}
		}
		public void Clear()
		{
			this.First = null;
			this.Count = 0;
		}
	}
}
