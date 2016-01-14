using System;
namespace SmartQuant
{
	public class Level2 : Tick
	{
		internal Level2Side side;
		internal Level2UpdateAction action;
		internal int position;
		public override byte TypeId
		{
			get
			{
				return 7;
			}
		}
		public Level2Side Side
		{
			get
			{
				return this.side;
			}
			set
			{
				this.side = value;
			}
		}
		public Level2UpdateAction Action
		{
			get
			{
				return this.action;
			}
			set
			{
				this.action = value;
			}
		}
		public int Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
			}
		}
		public Level2(DateTime dateTime, byte providerId, int instrumentId, double price, int size, Level2Side side, Level2UpdateAction action, int position) : base(dateTime, providerId, instrumentId, price, size)
		{
			this.side = side;
			this.action = action;
			this.position = position;
		}
		public Level2()
		{
		}
	}
}
