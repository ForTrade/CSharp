using System;
namespace SmartQuant
{
	public class LinkedListNode<T>
	{
		public T Data;
		public LinkedListNode<T> Next;
		public LinkedListNode(T data)
		{
			this.Data = data;
		}
	}
}
