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
	public class Invite : SqlObject<Invite> {

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "Invites";
			public const string FIELD_ID = "Id";
			public const string FIELD_CODE = "Code";
			public const string FIELD_ISUSED = "IsUsed";
			public const string FIELD_OWNERID = "OwnerId";
			public const string FIELD_GUESTID = "GuestId";
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) { Refresh(id); }
		}

		protected override ISqlObjectTableSpec table { get { return TableSpec.instance; } }

		private string _code;
		public string code {
			get {
				this.LoadIfNotLoaded();
				return this._code;
			}
		}

		private bool _isUsed;
		public bool isUsed {
			get {
				this.LoadIfNotLoaded();
				return this._isUsed;
			}
		}

		private int _ownerId;
		public int ownerId {
			get {
				this.LoadIfNotLoaded();
				return this._ownerId;
			}
		}
		public Account owner {
			get {
				return Account.LoadById(this.ownerId);
			}
		}

		private int? _invitedId;
		public int? invitedId {
			get {
				this.LoadIfNotLoaded();
				return this._invitedId;
			}
		}
		public Account invited {
			get {
				return Account.LoadById(this.invitedId.Value);
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._code = data[TableSpec.FIELD_CODE];
			this._isUsed = Util.string2bool(data[TableSpec.FIELD_ISUSED]);
			this._ownerId = int.Parse(data[TableSpec.FIELD_OWNERID]);
			this._invitedId = Util.ParseInt(data[TableSpec.FIELD_GUESTID]);
		}

		public XElement exportToXml(UserContext context) {
			return new XElement("invite",
				new XElement("id", this.id),
				new XElement("code", this.code),
				new XElement("isUsed", this.isUsed.ToPlainString())
			);
		}

		private object createAccount_locker = new object();
		public Account createAccount(string code, string name, string password) {
			lock(this.createAccount_locker) {
				if(this.isUsed) throw new FLocalException("Invite is already used");
				if(this.code != code) throw new FLocalException("Wrong code");
				var rawChanges = Account.getNewAccountChanges(name, password);
				var accountInsert = rawChanges.Key;
				var changes = new List<AbstractChange>(rawChanges.Value);
				changes.Add(
					new UpdateChange(
						TableSpec.instance,
						new Dictionary<string,AbstractFieldValue> {
							{ TableSpec.FIELD_ISUSED, new ScalarFieldValue("1") },
							{ TableSpec.FIELD_GUESTID, new ReferenceFieldValue(accountInsert) },
						},
						this.id
					)
				);
				ChangeSetUtil.ApplyChanges(changes.ToArray());
				return Account.LoadById(accountInsert.getId().Value);
			}
		}

	}
}
