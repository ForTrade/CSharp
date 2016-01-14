using System;
using System.Collections.Generic;
namespace SmartQuant
{
	public class AccountDataManager
	{
		private Framework framework;
		private Dictionary<int, AccountDataTable> tables;
		internal AccountDataManager(Framework framework)
		{
			this.framework = framework;
			this.tables = new Dictionary<int, AccountDataTable>();
		}
		internal void Clear()
		{
			lock (this.tables)
			{
				this.tables.Clear();
			}
		}
		internal void OnAccountData(AccountData data)
		{
			AccountDataTable table = this.GetTable(data.ProviderId, data.Route, true);
			lock (table)
			{
				AccountDataTableItem accountDataTableItem;
				if (!table.Items.TryGetValue(data.Account, out accountDataTableItem))
				{
					accountDataTableItem = new AccountDataTableItem();
					table.Items.Add(data.Account, accountDataTableItem);
				}
				switch (data.Type)
				{
				case AccountDataType.AccountValue:
					this.MergeFields(data.Fields, accountDataTableItem.Values);
					break;
				case AccountDataType.Position:
				{
					AccountDataKey key = new AccountDataKey(data, new string[]
					{
						"Symbol",
						"Maturity",
						"PutOrCall",
						"Strike"
					});
					AccountDataFieldList accountDataFieldList;
					if (!accountDataTableItem.Positions.TryGetValue(key, out accountDataFieldList))
					{
						accountDataFieldList = new AccountDataFieldList();
						accountDataTableItem.Positions.Add(key, accountDataFieldList);
					}
					accountDataFieldList.Clear();
					this.CopyFields(data.Fields, accountDataFieldList);
					break;
				}
				case AccountDataType.Order:
				{
					AccountDataKey key2 = new AccountDataKey(data, new string[]
					{
						"OrderID"
					});
					AccountDataFieldList accountDataFieldList2;
					if (!accountDataTableItem.Orders.TryGetValue(key2, out accountDataFieldList2))
					{
						accountDataFieldList2 = new AccountDataFieldList();
						accountDataTableItem.Orders.Add(key2, accountDataFieldList2);
					}
					accountDataFieldList2.Clear();
					this.CopyFields(data.Fields, accountDataFieldList2);
					break;
				}
				}
			}
		}
		public AccountDataSnapshot GetSnapshot(byte providerId, byte route)
		{
			AccountDataTable table = this.GetTable(providerId, route, false);
			if (table == null)
			{
				return new AccountDataSnapshot(new AccountDataEntry[0]);
			}
			AccountDataSnapshot result;
			lock (table)
			{
				List<AccountDataEntry> list = new List<AccountDataEntry>();
				foreach (string current in table.Items.Keys)
				{
					AccountDataTableItem accountDataTableItem = table.Items[current];
					AccountData accountData = new AccountData(this.framework.Clock.DateTime, AccountDataType.AccountValue, current, providerId, route);
					this.CopyFields(accountDataTableItem.Values, accountData.Fields);
					List<AccountData> list2 = new List<AccountData>();
					foreach (AccountDataFieldList current2 in accountDataTableItem.Positions.Values)
					{
						AccountData accountData2 = new AccountData(this.framework.Clock.DateTime, AccountDataType.Position, current, providerId, route);
						this.CopyFields(current2, accountData2.Fields);
						list2.Add(accountData2);
					}
					List<AccountData> list3 = new List<AccountData>();
					foreach (AccountDataFieldList current3 in accountDataTableItem.Orders.Values)
					{
						AccountData accountData3 = new AccountData(this.framework.Clock.DateTime, AccountDataType.Order, current, providerId, route);
						this.CopyFields(current3, accountData3.Fields);
						list3.Add(accountData3);
					}
					list.Add(new AccountDataEntry(current, accountData, list2.ToArray(), list3.ToArray()));
				}
				result = new AccountDataSnapshot(list.ToArray());
			}
			return result;
		}
		public AccountDataSnapshot[] GetSnapshots()
		{
			List<AccountDataSnapshot> list = new List<AccountDataSnapshot>();
			lock (this.tables)
			{
				foreach (int current in this.tables.Keys)
				{
					byte providerId = (byte)(current / 256);
					byte route = (byte)(current % 256);
					list.Add(this.GetSnapshot(providerId, route));
				}
			}
			return list.ToArray();
		}
		private AccountDataTable GetTable(byte providerId, byte route, bool addNew)
		{
			int key = (int)providerId * 256 + (int)route;
			AccountDataTable accountDataTable;
			lock (this.tables)
			{
				if (!this.tables.TryGetValue(key, out accountDataTable))
				{
					if (addNew)
					{
						accountDataTable = new AccountDataTable();
						this.tables.Add(key, accountDataTable);
					}
					else
					{
						accountDataTable = null;
					}
				}
			}
			return accountDataTable;
		}
		private void MergeFields(AccountDataFieldList srcList, AccountDataFieldList dstList)
		{
			foreach (AccountDataField accountDataField in srcList)
			{
				dstList[accountDataField.Name, accountDataField.Currency] = accountDataField.Value;
			}
		}
		private void CopyFields(AccountDataFieldList srcList, AccountDataFieldList dstList)
		{
			foreach (AccountDataField accountDataField in srcList)
			{
				dstList.Add(accountDataField.Name, accountDataField.Currency, accountDataField.Value);
			}
		}
	}
}
