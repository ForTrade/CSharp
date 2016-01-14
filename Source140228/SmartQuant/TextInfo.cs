using System;
namespace SmartQuant
{
	public class TextInfo : DataObject
	{
		public string Content
		{
			get;
			set;
		}
		public override byte TypeId
		{
			get
			{
				return 17;
			}
		}
		public TextInfo(DateTime dateTime, string content) : base(dateTime)
		{
			this.Content = content;
		}
		public TextInfo(DateTime dateTime, object data) : this(dateTime, data.ToString())
		{
		}
	}
}
