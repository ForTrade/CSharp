using System;
namespace SmartQuant
{
	internal interface IFillSet
	{
		void Push(Fill fill);
		Fill Pop();
		Fill Peek();
	}
}
