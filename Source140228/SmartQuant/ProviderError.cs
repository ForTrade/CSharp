using System;
namespace SmartQuant
{
	public class ProviderError : DataObject
	{
		internal ProviderErrorType type;
		internal byte providerId;
		internal int id;
		internal int code;
		internal string text;
		public override byte TypeId
		{
			get
			{
				return 21;
			}
		}
		public ProviderErrorType Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}
		public byte ProviderId
		{
			get
			{
				return this.providerId;
			}
			set
			{
				this.providerId = value;
			}
		}
		public int Id
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}
		public int Code
		{
			get
			{
				return this.code;
			}
			set
			{
				this.code = value;
			}
		}
		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
			}
		}
		public ProviderError(DateTime dateTime, ProviderErrorType type, byte providerId, int id, int code, string text) : base(dateTime)
		{
			this.type = type;
			this.providerId = providerId;
			this.id = id;
			this.code = code;
			this.text = text;
		}
		public ProviderError(DateTime dateTime, ProviderErrorType type, byte providerId, string text) : this(dateTime, type, providerId, -1, -1, text)
		{
		}
		public ProviderError()
		{
		}
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"id = ",
				this.id,
				" ",
				this.type,
				" code = ",
				this.code,
				" ",
				this.text
			});
		}
	}
}
