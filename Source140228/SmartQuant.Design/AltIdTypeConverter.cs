using System;
using System.ComponentModel;
using System.Globalization;
namespace SmartQuant.Design
{
	internal class AltIdTypeConverter : ExpandableObjectConverter
	{
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (!(value is AltId) || !(destinationType == typeof(string)))
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
			AltId altId = (AltId)value;
			if (string.IsNullOrEmpty(altId.symbol) && string.IsNullOrEmpty(altId.exchange))
			{
				return string.Empty;
			}
			return string.Format("{0}@{1}", altId.symbol, altId.exchange);
		}
	}
}
