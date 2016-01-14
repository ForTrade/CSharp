using System;
namespace SmartQuant
{
	public class GroupField
	{
		internal Group group;
		private object value;
		public string Name
		{
			get;
			private set;
		}
		public byte Type
		{
			get;
			private set;
		}
		public object Value
		{
			get
			{
				return this.value;
			}
			set
			{
				if (this.value != value)
				{
					object oldValue = this.value;
					this.value = value;
					this.group.Framework.eventServer.OnLog(new GroupUpdate(this.group.Id, this.Name, this.Type, this.value, oldValue, GroupUpdateType.FieldUpdated));
				}
			}
		}
		public GroupField(string name, byte type, object value)
		{
			this.Name = name;
			this.Type = type;
			this.value = value;
		}
	}
}
