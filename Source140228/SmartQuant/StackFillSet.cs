using System;
using System.Collections.Generic;
namespace SmartQuant
{
	internal class StackFillSet : IFillSet
	{
		private Stack<Fill> stack;
		public StackFillSet()
		{
			this.stack = new Stack<Fill>();
		}
		public void Push(Fill fill)
		{
			this.stack.Push(fill);
		}
		public Fill Pop()
		{
			return this.stack.Pop();
		}
		public Fill Peek()
		{
			if (this.stack.Count == 0)
			{
				return null;
			}
			return this.stack.Peek();
		}
	}
}
