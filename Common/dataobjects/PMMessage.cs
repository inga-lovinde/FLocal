using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Core.DB;
using FLocal.Common;
using FLocal.Common.actions;

namespace FLocal.Common.dataobjects {
	public class PMMessage : SqlObject<PMMessage> {

		public const string ENUM_DIRECTION_INCOMING = "Incoming";
		public const string ENUM_DIRECTION_OUTGOING = "Outgoing";

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "PMMessages";
			public const string FIELD_ID = "Id";
			public const string FIELD_OWNERID = "OwnerId";
			public const string FIELD_INTERLOCUTORID = "InterlocutorId";
			public const string FIELD_DIRECTION = "Direction";
			public const string FIELD_POSTDATE = "PostDate";
			public const string FIELD_TITLE = "Title";
			public const string FIELD_BODY = "Body";
			public const string FIELD_BODYUBB = "BodyUBB";
			public const string FIELD_INCOMINGPMID = "IncomingPMId";
			public const string FIELD_ISREAD = "IsRead";
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) { Refresh(id); }
		}

		protected override ISqlObjectTableSpec table { get { return TableSpec.instance; } }

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

		private int _interlocutorId;
		public int interlocutorId {
			get {
				this.LoadIfNotLoaded();
				return this._interlocutorId;
			}
		}
		public Account interlocutor {
			get {
				return Account.LoadById(this.interlocutorId);
			}
		}

		public Account poster {
			get {
				if(this.direction == ENUM_DIRECTION_INCOMING) {
					return this.interlocutor;
				} else {
					return this.owner;
				}
			}
		}

		private string _direction;
		public string direction {
			get {
				this.LoadIfNotLoaded();
				return this._direction;
			}
		}

		private DateTime _postDate;
		public DateTime postDate {
			get {
				this.LoadIfNotLoaded();
				return this._postDate;
			}
		}

		private string _title;
		public string title {
			get {
				this.LoadIfNotLoaded();
				return this._title;
			}
		}

		private string _body;
		public string body {
			get {
				this.LoadIfNotLoaded();
				return this._body;
			}
		}

		private string _bodyUBB;
		public string bodyUBB {
			get {
				this.LoadIfNotLoaded();
				return this._bodyUBB;
			}
		}

		private int? _incomingPMId;
		public int? incomingPMId {
			get {
				this.LoadIfNotLoaded();
				return this._incomingPMId;
			}
		}

		private bool _isRead;
		public bool isRead {
			get {
				this.LoadIfNotLoaded();
				return this._isRead;
			}
		}

		public PMConversation conversation {
			get {
				return PMConversation.LoadByAccounts(this.owner, this.interlocutor);
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._ownerId = int.Parse(data[TableSpec.FIELD_OWNERID]);
			this._interlocutorId = int.Parse(data[TableSpec.FIELD_INTERLOCUTORID]);
			this._direction = data[TableSpec.FIELD_DIRECTION];
			this._postDate = Util.ParseDateTimeFromTimestamp(data[TableSpec.FIELD_POSTDATE]).Value;
			this._title = data[TableSpec.FIELD_TITLE];
			this._body = data[TableSpec.FIELD_BODY];
			this._bodyUBB = data[TableSpec.FIELD_BODYUBB];
			this._incomingPMId = Util.ParseInt(data[TableSpec.FIELD_INCOMINGPMID]);
			this._isRead = Util.string2bool(data[TableSpec.FIELD_ISREAD]);
		}

		public XElement exportToXml(UserContext context, params XElement[] additional) {
			if((context.account == null) || (context.account.id != this.owner.id)) {
				throw new AccessViolationException();
			}

			XElement result = new XElement("message",
				new XElement("id", this.id),
				new XElement("poster", this.poster.exportToXml(context)),
				new XElement("owner", this.owner.exportToXml(context)),
				new XElement("interlocutor", this.interlocutor.exportToXml(context)),
				new XElement("isRead", this.isRead.ToPlainString()),
				new XElement("postDate", this.postDate.ToXml()),
				new XElement("title", this.title),
				new XElement("body", context.outputParams.preprocessBodyIntermediate(this.body)),
				new XElement("bodyUBB", this.bodyUBB)
			);
			if(additional.Length > 0) {
				result.Add(additional);
			}
			return result;
		}

		private readonly object MarkAsRead_locker = new object();
		public void MarkAsRead(Account account) {
			if(account.id != this.owner.id) throw new AccessViolationException();
			if(!this.isRead) {
				lock(MarkAsRead_locker) {
					//so we can safely decrease ReadPrivateMessages counter
					//Note that this code implicitly uses assumption of single-instance Common.dll; race condition is possible with more than one server
					if(!this.isRead) {
						AccountIndicator indicator = AccountIndicator.LoadByAccount(this.owner);
						ChangeSetUtil.ApplyChanges(
							new UpdateChange(
								TableSpec.instance,
								new Dictionary<string,AbstractFieldValue> {
									{ TableSpec.FIELD_ISREAD, new ScalarFieldValue("1") },
								},
								this.id
							),
							new UpdateChange(
								AccountIndicator.TableSpec.instance,
								new Dictionary<string,AbstractFieldValue> {
									{ AccountIndicator.TableSpec.FIELD_UNREADPRIVATEMESSAGES, new IncrementFieldValue(IncrementFieldValue.DECREMENTOR) },
								},
								indicator.id
							)
						);
					}
				}
			}
		}

	}
}
