using System;
namespace SmartQuant
{
	public class StrategyStatusInfo : DataObject
	{
		public StrategyStatusType Type
		{
			get;
			private set;
		}
		public string Solution
		{
			get;
			set;
		}
		public string Mode
		{
			get;
			set;
		}
		public override byte TypeId
		{
			get
			{
				return 20;
			}
		}
		public StrategyStatusInfo(DateTime dateTime, StrategyStatusType type)
		{
			this.Type = type;
			base.DateTime = dateTime;
		}
	}
}
