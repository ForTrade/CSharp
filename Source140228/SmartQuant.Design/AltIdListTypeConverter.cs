using System;
using System.ComponentModel;
using System.Globalization;
namespace SmartQuant.Design
{
	internal class AltIdListTypeConverter : ArrayConverter
	{
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value is AltIdList && destinationType == typeof(string))
			{
				return string.Format("{0} item(s)", ((AltIdList)value).Count);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			if (value is AltIdList)
			{
				PropertyDescriptorCollection propertyDescriptorCollection = new PropertyDescriptorCollection(null);
				AltIdList altIdList = (AltIdList)value;
				foreach (AltId current in altIdList)
				{
					propertyDescriptorCollection.Add(new AltIdPropertyDescriptor(current));
				}
				return propertyDescriptorCollection;
			}
			return base.GetProperties(context, value, attributes);
		}
	}
}
