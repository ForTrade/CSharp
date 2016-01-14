using System;
namespace SmartQuant
{
	public class GroupUpdate : DataObject
	{
		private int groupId;
		private string fieldName;
		private GroupUpdateType updateType;
		private byte fieldType;
		private object value;
		private object oldValue;
		public override byte TypeId
		{
			get
			{
				return 51;
			}
		}
		public int GroupId
		{
			get
			{
				return this.groupId;
			}
		}
		public string FieldName
		{
			get
			{
				return this.fieldName;
			}
		}
		public GroupUpdateType UpdateType
		{
			get
			{
				return this.updateType;
			}
		}
		public byte FieldType
		{
			get
			{
				return this.fieldType;
			}
		}
		public object Value
		{
			get
			{
				return this.value;
			}
		}
		public object OldValue
		{
			get
			{
				return this.oldValue;
			}
		}
		public GroupUpdate(int groupId, string fieldName, byte fieldType, object value, object oldValue, GroupUpdateType updateType)
		{
			this.groupId = groupId;
			this.fieldName = fieldName;
			this.fieldType = fieldType;
			this.value = value;
			this.oldValue = oldValue;
			this.updateType = updateType;
		}
	}
}
