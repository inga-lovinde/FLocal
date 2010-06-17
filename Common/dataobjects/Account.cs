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
	public class Account : SqlObject<Account> {

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "Accounts";
			public const string FIELD_ID = "Id";
			public const string FIELD_USERID = "UserId";
			public const string FIELD_PASSWORD = "Password";
			public const string FIELD_NEEDSMIGRATION = "NeedsMigration";
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) { Refresh(id); }
		}

		protected override ISqlObjectTableSpec table { get { return TableSpec.instance; } }

		private int _userId;
		public int userId {
			get {
				this.LoadIfNotLoaded();
				return this._userId;
			}
		}
		public User user {
			get {
				return User.LoadById(this.userId);
			}
		}

		private string _password;
		private string password {
			get {
				this.LoadIfNotLoaded();
				return this._password;
			}
		}

		private bool _needsMigration;
		public bool needsMigration {
			get {
				this.LoadIfNotLoaded();
				return this._needsMigration;
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._userId = int.Parse(data[TableSpec.FIELD_USERID]);
			this._password = data[TableSpec.FIELD_PASSWORD];
			this._needsMigration = Util.string2bool(data[TableSpec.FIELD_NEEDSMIGRATION]);
		}

		private static Dictionary<int, int> userid2id = new Dictionary<int, int>();
		public static Account LoadByUser(User user) {
			if(!userid2id.ContainsKey(user.id)) {
				lock(userid2id) {
					if(!userid2id.ContainsKey(user.id)) {
						List<string> ids = Config.instance.mainConnection.LoadIdsByConditions(
							TableSpec.instance,
							new ComparisonCondition(
								TableSpec.instance.getColumnSpec(TableSpec.FIELD_USERID),
								ComparisonType.EQUAL,
								user.id.ToString()
							),
							Diapasone.unlimited,
							new JoinSpec[0]
						);
						if(ids.Count > 1) {
							throw new CriticalException("not unique");
						} else if(ids.Count == 1) {
							userid2id[user.id] = int.Parse(ids[0]);
						} else {
							throw new NotFoundInDBException();
						}
					}
				}
			}
			return Account.LoadById(userid2id[user.id]);
		}

		public void updatePassword(string newPassword) {
			using(ChangeSet changeSet = new ChangeSet()) {
				changeSet.Add(
					new UpdateChange(
						TableSpec.instance,
						new Dictionary<string, AbstractFieldValue>() {
							{
								TableSpec.FIELD_PASSWORD,
								new ScalarFieldValue(Util.md5(newPassword + " " + Config.instance.SaltMigration + " " + this.id))
							},
							{
								TableSpec.FIELD_NEEDSMIGRATION,
								new ScalarFieldValue("0")
							},
						},
						this.id
					)
				);
				using(Transaction transaction = Config.instance.mainConnection.beginTransaction()) {
					changeSet.Apply(transaction);
					transaction.Commit();
				}
			}
		}

	}
}
