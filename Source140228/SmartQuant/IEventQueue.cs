using System;
namespace SmartQuant
{
	public interface IEventQueue
	{
		byte Id
		{
			get;
		}
		byte Type
		{
			get;
		}
		string Name
		{
			get;
		}
		byte Priority
		{
			get;
		}
		long Count
		{
			get;
		}
		long FullCount
		{
			get;
		}
		long EmptyCount
		{
			get;
		}
		Event Peek();
		DateTime PeekDateTime();
		Event Read();
		void Write(Event obj);
		Event Dequeue();
		void Enqueue(Event obj);
		bool IsEmpty();
		bool IsFull();
		void Clear();
		void ResetCounts();
	}
}
