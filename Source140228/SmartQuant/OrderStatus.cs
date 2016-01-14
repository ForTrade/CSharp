using System;
namespace SmartQuant
{
	public enum OrderStatus : byte
	{
		NotSent,
		PendingNew,
		New,
		Rejected,
		PartiallyFilled,
		Filled,
		PendingCancel,
		Cancelled,
		Expired,
		PendingReplace,
		Replaced
	}
}
