using System;
namespace SmartQuant
{
	public class News : DataObject
	{
		internal int providerId;
		internal int instrumentId;
		internal byte urgency;
		internal string url;
		internal string headline;
		internal string text;
		public override byte TypeId
		{
			get
			{
				return 23;
			}
		}
		public News()
		{
		}
		public News(DateTime dateTime, int providerId, int instrumentId, byte urgency, string url, string headline, string text) : base(dateTime)
		{
			this.providerId = providerId;
			this.instrumentId = instrumentId;
			this.urgency = urgency;
			this.url = url;
			this.headline = headline;
			this.text = text;
		}
		public override string ToString()
		{
			return this.headline + " : " + this.text;
		}
	}
}
