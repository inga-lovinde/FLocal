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
	public class Session : SqlObject<Session.SessionKey, Session> {

		public struct SessionKey : IEquatable<SessionKey> {

			private string key;
			
			public static SessionKey Parse(string _key) {
				return new SessionKey { key = _key };
			}

			bool IEquatable<SessionKey>.Equals(SessionKey _key) {
				return this.key.Equals(_key.key);
			}

			public override bool Equals(object obj) {
				if(obj == null) return base.Equals(obj);
				if(!(obj is SessionKey)) throw new InvalidCastException();
				return ((IEquatable<SessionKey>)this).Equals((SessionKey)obj);
			}

			public override int GetHashCode() {
				return this.key.GetHashCode();
			}

			public override string ToString() {
				return this.key;
			}

		}

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "Sessions";
			public const string FIELD_ID = FIELD_SESSIONKEY;
			public const string FIELD_SESSIONKEY = "SessionKey";
			public const string FIELD_ACCOUNTID = "AccountId";
			public const string FIELD_LASTACTIVITY = "LastActivity";
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) { throw new NotImplementedException(); }
		}

		protected override ISqlObjectTableSpec table { get { return TableSpec.instance; } }

		private string _sessionKey;
		public string sessionKey {
			get {
				this.LoadIfNotLoaded();
				return this._sessionKey;
			}
		}

		private int _accountId;
		public int accountId {
			get {
				this.LoadIfNotLoaded();
				return this._accountId;
			}
		}
		public Account account {
			get {
				return Account.LoadById(this.accountId);
			}
		}

		private DateTime _lastActivity;
		public DateTime lastActivity {
			get {
				this.LoadIfNotLoaded();
				return this._lastActivity;
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._sessionKey = data[TableSpec.FIELD_SESSIONKEY];
			this._lastActivity = new DateTime(long.Parse(data[TableSpec.FIELD_LASTACTIVITY]));
			this._accountId = int.Parse(data[TableSpec.FIELD_ACCOUNTID]);
		}

		public void updateLastActivity() {
			try {
				Config.Transactional(transaction => {
					Config.instance.mainConnection.update(
						transaction,
						TableSpec.instance,
						this.id.ToString(),
						new Dictionary<string,string>() {
							{ TableSpec.FIELD_LASTACTIVITY, DateTime.Now.ToUTCString() }
						}
					);
				});
			} finally {
				this.ReLoad();
			}
		}

		public static Session create(Account account) {

			string key=null;

			Config.Transactional(transaction => {
	
				Config.instance.mainConnection.lockTable(transaction, TableSpec.instance);
				
				do {
					key = Util.RandomString(32, Util.RandomSource.LETTERS_DIGITS);
				} while(Config.instance.mainConnection.LoadByIds(transaction, TableSpec.instance, new List<string>() { key }).Count > 0);

				Config.instance.mainConnection.insert(
					transaction,
					TableSpec.instance,
					new Dictionary<string,string>() {
						{ TableSpec.FIELD_SESSIONKEY, key },
						{ TableSpec.FIELD_ACCOUNTID, account.id.ToString() },
						{ TableSpec.FIELD_LASTACTIVITY, DateTime.Now.ToUTCString() },
					}
				);
			});
			return Session.LoadById(SessionKey.Parse(key));
		}

		public void delete() {
			Config.Transactional(transaction => Config.instance.mainConnection.delete(transaction, TableSpec.instance, this.id.ToString()));
			this.deleteFromRegistry();
		}

		public XElement exportToXml(UserContext context) {
			return new XElement("session",
				new XElement("lastActivity", this.lastActivity.ToXml()),
				new XElement("sessionKey", this.sessionKey),
				this.account.user.exportToXmlForViewing(context)
			);
		}

		public static Session LoadByKey(string sessionKey) {
			return LoadById(SessionKey.Parse(sessionKey));
		}

	}
}
