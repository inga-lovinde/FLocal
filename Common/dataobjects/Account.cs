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
			public const string FIELD_IPADDRESS = "IpAddress";
			public const string FIELD_REGISTRATIONEMAIL = "RegistrationEmail";
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

		private string _name;
		public string name {
			get {
				this.LoadIfNotLoaded();
				return this._name;
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._userId = int.Parse(data[TableSpec.FIELD_USERID]);
			this._passwordHash = data[TableSpec.FIELD_PASSWORDHASH];
			this._needsMigration = Util.string2bool(data[TableSpec.FIELD_NEEDSMIGRATION]);
			this._name = data[TableSpec.FIELD_NAME];
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
						name2id[name] = int.Parse(
							Config.instance.mainConnection.LoadIdByField(
								TableSpec.instance.getColumnSpec(TableSpec.FIELD_NAME),
								name
							)
						);
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
						userid2id[user.id] = int.Parse(
							Config.instance.mainConnection.LoadIdByField(
								TableSpec.instance.getColumnSpec(TableSpec.FIELD_USERID),
								user.id.ToString()
							)
						);
					}
				}
			}
			return Account.LoadById(userid2id[user.id]);
		}

		private string hashPasswordLegacy(string password) {
			return Util.md5(Util.md5(password) + " " + Util.md5(Config.instance.SaltMigration) + " " + Util.md5(this.id.ToString()));
		}

		private static string hashPassword(string password, string name) {
			return Util.md5(Util.md5(password) + " " + Util.md5(Config.instance.SaltMigration) + " " + Util.md5(name));
		}

		private string hashPassword(string password) {
			return hashPassword(password, name);
		}

		public bool checkPassword(string password) {
			return (this.hashPassword(password) == this.passwordHash) || (this.hashPasswordLegacy(password) == this.passwordHash);
		}

		public void updatePassword(string newPassword) {
			checkNewPassword(newPassword);
			ChangeSetUtil.ApplyChanges(new AbstractChange[] {
				new UpdateChange(
					TableSpec.instance,
					new Dictionary<string, AbstractFieldValue>() {
						{
							TableSpec.FIELD_PASSWORDHASH,
							new ScalarFieldValue(this.hashPasswordLegacy(newPassword))
						},
					},
					this.id
				)
			});
		}

		public void updateRegistrationEmail(string newEmail) {
			ChangeSetUtil.ApplyChanges(new AbstractChange[] {
				new UpdateChange(
					TableSpec.instance,
					new Dictionary<string, AbstractFieldValue>() {
						{
							TableSpec.FIELD_REGISTRATIONEMAIL,
							new ScalarFieldValue(newEmail)
						},
					},
					this.id
				)
			});
		}

		public static void checkNewPassword(string newPassword) {
			if(newPassword.Length < 5) throw new FLocalException("Password is too short");
		}

		public static void checkNewName(string newName) {
			if(newName.Length < 2) throw new FLocalException("Name is too short");
			if(newName.Length > 16) throw new FLocalException("Name is too long");
			try {
				Account.LoadByName(newName);
				throw new FLocalException("Name is already used");
			} catch(NotFoundInDBException) {
			}
		}

		public static KeyValuePair<AbstractChange, AbstractChange[]> getNewAccountChanges(string _name, string password, string ip, string registrationEmail) {
			string name = _name.Trim();
			checkNewName(name);
			checkNewPassword(password);
			var userInsert = new InsertChange(
				User.TableSpec.instance,
				new Dictionary<string, AbstractFieldValue> {
					{ User.TableSpec.FIELD_AVATARID, new ScalarFieldValue(null) },
					{ User.TableSpec.FIELD_BIOGRAPHY, new ScalarFieldValue("") },
					{ User.TableSpec.FIELD_LOCATION, new ScalarFieldValue("") },
					{ User.TableSpec.FIELD_NAME, new ScalarFieldValue(name) },
					{ User.TableSpec.FIELD_REGDATE, new ScalarFieldValue(DateTime.Now.ToUTCString()) },
					{ User.TableSpec.FIELD_SHOWPOSTSTOUSERS, new ScalarFieldValue(User.ENUM_SHOWPOSTSTOUSERS_ALL) },
					{ User.TableSpec.FIELD_SIGNATURE, new ScalarFieldValue("") },
					{ User.TableSpec.FIELD_TITLE, new ScalarFieldValue("novice") },
					{ User.TableSpec.FIELD_TOTALPOSTS, new ScalarFieldValue("0") },
					{ User.TableSpec.FIELD_USERGROUPID, new ScalarFieldValue("1") },
				}
			);
			var accountInsert = new InsertChange(
				Account.TableSpec.instance,
				new Dictionary<string, AbstractFieldValue> {
					{ Account.TableSpec.FIELD_NAME, new ScalarFieldValue(name.ToLower()) },
					{ Account.TableSpec.FIELD_NEEDSMIGRATION, new ScalarFieldValue("0") },
					{ Account.TableSpec.FIELD_PASSWORDHASH, new ScalarFieldValue(hashPassword(password, name.ToLower())) },
					{ Account.TableSpec.FIELD_USERID, new ReferenceFieldValue(userInsert) },
					{ Account.TableSpec.FIELD_IPADDRESS, new ScalarFieldValue(ip) },
					{ Account.TableSpec.FIELD_REGISTRATIONEMAIL, new ScalarFieldValue(registrationEmail) },
				}
			);
			var indicatorInsert = new InsertChange(
				AccountIndicator.TableSpec.instance,
				new Dictionary<string,AbstractFieldValue> {
					{ AccountIndicator.TableSpec.FIELD_ACCOUNTID, new ReferenceFieldValue(accountInsert) },
					{ AccountIndicator.TableSpec.FIELD_PRIVATEMESSAGES, new ScalarFieldValue("0") },
					{ AccountIndicator.TableSpec.FIELD_UNREADPRIVATEMESSAGES, new ScalarFieldValue("0") },
				}
			);
			return new KeyValuePair<AbstractChange,AbstractChange[]>(
				accountInsert,
				new[] {
					userInsert,
					accountInsert,
					indicatorInsert,
				}
			);
		}

		public void migrate(string newPassword, string ip, string registrationEmail) {
			checkNewPassword(newPassword);
			if(!this.needsMigration) throw new FLocalException("Already migrated");
			ChangeSetUtil.ApplyChanges(new AbstractChange[] {
				new UpdateChange(
					TableSpec.instance,
					new Dictionary<string, AbstractFieldValue>() {
						{
							TableSpec.FIELD_PASSWORDHASH,
							new ScalarFieldValue(this.hashPasswordLegacy(newPassword))
						},
						{
							TableSpec.FIELD_NEEDSMIGRATION,
							new ScalarFieldValue("0")
						},
						{
							TableSpec.FIELD_IPADDRESS,
							new ScalarFieldValue(ip)
						},
						{
							TableSpec.FIELD_REGISTRATIONEMAIL,
							new ScalarFieldValue(registrationEmail)
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
			if(!account.checkPassword(password)) throw new FLocalException("Wrong password (" + account.id + ":" + account.hashPassword(password) + ")");
			return account;
		}

		public static Account createAccount(string name, string password, string ip, string registrationEmail) {
			var rawChanges = Account.getNewAccountChanges(name, password, ip, registrationEmail);
			var accountInsert = rawChanges.Key;
			var changes = rawChanges.Value;
			ChangeSetUtil.ApplyChanges(changes);
			return Account.LoadById(accountInsert.getId().Value);
		}

	}
}
