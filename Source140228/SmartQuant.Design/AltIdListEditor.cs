using System;
using System.Collections;
using System.ComponentModel.Design;
namespace SmartQuant.Design
{
	internal class AltIdListEditor : ArrayEditor
	{
		public AltIdListEditor() : base(typeof(ArrayList))
		{
		}
		protected override object[] GetItems(object editValue)
		{
			if (editValue is AltIdList)
			{
				ArrayList arrayList = new ArrayList();
				foreach (AltId current in (AltIdList)editValue)
				{
					arrayList.Add(new AltId(current.providerId, current.symbol, current.exchange));
				}
				return arrayList.ToArray();
			}
			return base.GetItems(editValue);
		}
		protected override object SetItems(object editValue, object[] value)
		{
			if (editValue is AltIdList)
			{
				AltIdList altIdList = (AltIdList)editValue;
				altIdList.Clear();
				for (int i = 0; i < value.Length; i++)
				{
					AltId id = (AltId)value[i];
					altIdList.Add(id);
				}
				return editValue;
			}
			return base.SetItems(editValue, value);
		}
		protected override Type CreateCollectionItemType()
		{
			return typeof(AltId);
		}
		protected override object CreateInstance(Type itemType)
		{
			if (itemType == typeof(AltId))
			{
				return new AltId(0, string.Empty, string.Empty);
			}
			return base.CreateInstance(itemType);
		}
	}
}
