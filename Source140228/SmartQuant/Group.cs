using System;
using System.Collections.Generic;
using System.Drawing;
namespace SmartQuant
{
	public class Group : DataObject
	{
		public int Id
		{
			get;
			internal set;
		}
		public string Name
		{
			get;
			private set;
		}
		public Dictionary<string, GroupField> Fields
		{
			get;
			private set;
		}
		public Framework Framework
		{
			get;
			internal set;
		}
		public List<GroupEvent> Events
		{
			get;
			private set;
		}
		public override byte TypeId
		{
			get
			{
				return 50;
			}
		}
		public Group(string name)
		{
			this.Fields = new Dictionary<string, GroupField>();
			this.Name = name;
			this.Events = new List<GroupEvent>();
		}
		public void Add(string name, byte type, object value)
		{
			this.Add(new GroupField(name, type, value));
		}
		public void Add(string name, Color color)
		{
			this.Add(new GroupField(name, 156, color));
		}
		public void Add(string name, string value)
		{
			this.Add(new GroupField(name, 150, value));
		}
		public void Add(string name, int value)
		{
			this.Add(new GroupField(name, 152, value));
		}
		public void Add(string name, bool value)
		{
			this.Add(new GroupField(name, 155, value));
		}
		public void Add(string name, DateTime dateTime)
		{
			this.Add(new GroupField(name, 153, dateTime));
		}
		private void Add(GroupField field)
		{
			this.Fields[field.Name] = field;
			field.group = this;
		}
		public void Remove(string fieldName)
		{
			this.Fields.Remove(fieldName);
		}
		public void OnNewGroupEvent(GroupEvent groupEvent)
		{
			this.Events.Add(groupEvent);
		}
	}
}
