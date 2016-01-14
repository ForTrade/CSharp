using System;
using System.Collections.Generic;
namespace SmartQuant
{
	internal class QueueFillSet : IFillSet
	{
		private Queue<Fill> queue;
		public QueueFillSet()
		{
			this.queue = new Queue<Fill>();
		}
		public Fill Pop()
		{
			return this.queue.Dequeue();
		}
		public void Push(Fill fill)
		{
			this.queue.Enqueue(fill);
		}
		public Fill Peek()
		{
			if (this.queue.Count == 0)
			{
				return null;
			}
			return this.queue.Peek();
		}
	}
}
