using System;
namespace SmartQuant
{
	public class DataMessageType
	{
		public const byte Open = 0;
		public const byte Close = 1;
		public const byte ReadBuffer = 2;
		public const byte WriteBuffer = 3;
		public const byte Flush = 4;
	}
}
