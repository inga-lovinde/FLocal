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
			public const string FIELD_PASSWORDHASH = "Password";
			public const string FIELD_NEEDSMIGRATION = "NeedsMigration";
			public const string FIELD_NAME = "Name";
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

		private string _passwordHash;
		private string passwordHash {
			get {
				this.LoadIfNotLoaded();
				return this._passwordHash;
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
			this._passwordHash = data[TableSpec.FIELD_PASSWORDHASH];
			this._needsMigration = Util.string2bool(data[TableSpec.FIELD_NEEDSMIGRATION]);
		}

		public XElement exportToXml(UserContext context) {
			return new XElement("account",
				new XElement("id", this.id),
				this.user.exportToXmlForViewing(context)
			);
		}

		private static Dictionary<string, int> name2id = new Dictionary<string, int>();
		public static Account LoadByName(string _name) {
			string name = _name.ToLower();
			if(!name2id.ContainsKey(name)) {
				lock(name2id) {
					if(!name2id.ContainsKey(name)) {
						List<string> ids = Config.instance.mainConnection.LoadIdsByConditions(
							TableSpec.instance,
							new ComparisonCondition(
								TableSpec.instance.getColumnSpec(TableSpec.FIELD_NAME),
								ComparisonType.EQUAL,
								name
							),
							Diapasone.unlimited,
							new JoinSpec[0]
						);
						if(ids.Count > 1) {
							throw new CriticalException("not unique");
						} else if(ids.Count == 1) {
							name2id[name] = int.Parse(ids[0]);
						} else {
							throw new NotFoundInDBException();
						}
					}
				}
			}
			return Account.LoadById(name2id[name]);
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
							Diapasone.unlimited
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

		private string hashPassword(string password) {
			return Util.md5(password + " " + Config.instance.SaltMigration + " " + this.id);
		}

		public bool checkPassword(string password) {
			return this.hashPassword(password) == this.passwordHash;
		}

		public void updatePassword(string newPassword) {
			ChangeSetUtil.ApplyChanges(new AbstractChange[] {
				new UpdateChange(
					TableSpec.instance,
					new Dictionary<string, AbstractFieldValue>() {
						{
							TableSpec.FIELD_PASSWORDHASH,
							new ScalarFieldValue(this.hashPassword(newPassword))
						},
					},
					this.id
				)
			});
		}

		public void migrate(string newPassword) {
			ChangeSetUtil.ApplyChanges(new AbstractChange[] {
				new UpdateChange(
					TableSpec.instance,
					new Dictionary<string, AbstractFieldValue>() {
						{
							TableSpec.FIELD_PASSWORDHASH,
							new ScalarFieldValue(this.hashPassword(newPassword))
						},
						{
							TableSpec.FIELD_NEEDSMIGRATION,
							new ScalarFieldValue("0")
						},
					},
					this.id
				),
				new UpdateChange(
					User.TableSpec.instance,
					new Dictionary<string, AbstractFieldValue> {
						{ User.TableSpec.FIELD_SHOWPOSTSTOUSERS, new ScalarFieldValue(User.ENUM_SHOWPOSTSTOUSERS_ALL) },
					},
					this.user.id
				)
			});
		}

		public static Account tryAuthorize(string name, string password) {
			Account account = LoadByName(name);
			if(account.passwordHash != account.hashPassword(password)) throw new NotFoundInDBException();
			return account;
		}

	}
}
