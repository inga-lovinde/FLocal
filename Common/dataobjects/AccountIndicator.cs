using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Core.DB;
using FLocal.Core.DB.conditions;
using FLocal.Common.actions;

namespace FLocal.Common.dataobjects {
	public class AccountIndicator : SqlObject<AccountIndicator> {

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "Accounts_Indicators";
			public const string FIELD_ID = "Id";
			public const string FIELD_ACCOUNTID = "AccountId";
			public const string FIELD_PRIVATEMESSAGES = "PrivateMessages";
			public const string FIELD_UNREADPRIVATEMESSAGES = "UnreadPrivateMessages";
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) { Refresh(id); }
		}

		protected override ISqlObjectTableSpec table { get { return TableSpec.instance; } }

		private int _accountId;
		public int accountId {
			get {
				this.LoadIfNotLoaded();
				return this._accountId;
			}
		}
		public Account account {
			get {
				return Account.LoadById(this._accountId);
			}
		}

		private int _privateMessages;
		public int privateMessages {
			get {
				this.LoadIfNotLoaded();
				return this._privateMessages;
			}
		}

		private int _unreadPrivateMessages;
		public int unreadPrivateMessages {
			get {
				this.LoadIfNotLoaded();
				return this._unreadPrivateMessages;
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._accountId = int.Parse(data[TableSpec.FIELD_ACCOUNTID]);
			this._privateMessages = int.Parse(data[TableSpec.FIELD_PRIVATEMESSAGES]);
			this._unreadPrivateMessages = int.Parse(data[TableSpec.FIELD_UNREADPRIVATEMESSAGES]);
		}

		private static Dictionary<int, int> accountid2id = new Dictionary<int, int>();
		public static AccountIndicator LoadByAccount(Account account) {
			if(!accountid2id.ContainsKey(account.id)) {
				lock(accountid2id) {
					if(!accountid2id.ContainsKey(account.id)) {
						List<string> ids = Config.instance.mainConnection.LoadIdsByConditions(
							TableSpec.instance,
							new ComparisonCondition(
								TableSpec.instance.getColumnSpec(TableSpec.FIELD_ACCOUNTID),
								ComparisonType.EQUAL,
								account.id.ToString()
							),
							Diapasone.unlimited,
							new JoinSpec[0]
						);
						if(ids.Count > 1) {
							throw new CriticalException("not unique");
						} else if(ids.Count == 1) {
							accountid2id[account.id] = int.Parse(ids[0]);
						} else {
							throw new CriticalException("not found");
						}
					}
				}
			}
			return AccountIndicator.LoadById(accountid2id[account.id]);
		}

		public XElement exportToXml(UserContext context) {
			return new XElement("indicators",
				new XElement("privateMessages", this.privateMessages),
				new XElement("unreadPrivateMessages", this.unreadPrivateMessages)
			);
		}

	}
}
