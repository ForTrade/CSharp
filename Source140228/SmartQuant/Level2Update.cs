using System;
namespace SmartQuant
{
	public class Level2Update : DataObject
	{
		internal byte providerId;
		internal int instrumentId;
		internal Level2[] entries;
		public override byte TypeId
		{
			get
			{
				return 9;
			}
		}
		public Level2[] Entries
		{
			get
			{
				return this.entries;
			}
		}
		public Level2Update(DateTime dateTime, byte providerId, int instrumentId, Level2[] entries) : base(dateTime)
		{
			this.providerId = providerId;
			this.instrumentId = instrumentId;
			this.entries = entries;
		}
		public Level2Update()
		{
		}
	}
}
