using System;
namespace SmartQuant
{
	public class HistoricalDataEnd : Event
	{
		public override byte TypeId
		{
			get
			{
				return 131;
			}
		}
		public string RequestId
		{
			get;
			set;
		}
		public RequestResult Result
		{
			get;
			set;
		}
		public string Text
		{
			get;
			set;
		}
	}
}
