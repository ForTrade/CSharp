using System;
using System.ComponentModel;
namespace SmartQuant.Design
{
	internal class AltIdPropertyDescriptor : PropertyDescriptor
	{
		private AltId altId;
		public override Type ComponentType
		{
			get
			{
				return typeof(AltIdList);
			}
		}
		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}
		public override Type PropertyType
		{
			get
			{
				return typeof(AltId);
			}
		}
		public override TypeConverter Converter
		{
			get
			{
				return new AltIdTypeConverter();
			}
		}
		public override string DisplayName
		{
			get
			{
				return string.Format("[{0}]", this.altId.providerId);
			}
		}
		public AltIdPropertyDescriptor(AltId altId) : base(altId.providerId.ToString(), null)
		{
			this.altId = altId;
		}
		public override bool CanResetValue(object component)
		{
			return true;
		}
		public override object GetValue(object component)
		{
			return this.altId;
		}
		public override void ResetValue(object component)
		{
		}
		public override void SetValue(object component, object value)
		{
		}
		public override bool ShouldSerializeValue(object component)
		{
			return true;
		}
	}
}
