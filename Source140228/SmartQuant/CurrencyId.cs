using System;
namespace SmartQuant
{
	public class CurrencyId
	{
		public const byte USD = 1;
		public const byte EUR = 2;
		public const byte RUR = 3;
		internal static IdName idName = new IdName();
		public static byte GetId(string name)
		{
			return CurrencyId.idName.GetId(name);
		}
		public static string GetName(byte id)
		{
			return CurrencyId.idName.GetName(id);
		}
	}
}
