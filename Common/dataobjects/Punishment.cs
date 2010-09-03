using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Core.DB;
using FLocal.Core.DB.conditions;

namespace FLocal.Common.dataobjects {
	public class Punishment : SqlObject<Punishment> {

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "Punishments";
			public const string FIELD_ID = "Id";
			public const string FIELD_POSTID = "PostId";
			public const string FIELD_OWNERID = "OwnerId";
			public const string FIELD_ORIGINALBOARDID = "OriginalBoardId";
			public const string FIELD_MODERATORID = "ModeratorId";
			public const string FIELD_PUNISHMENTDATE = "PunishmentDate";
			public const string FIELD_PUNISHMENTTYPE = "PunishmentType";
			public const string FIELD_ISWITHDRAWED = "IsWithdrawed";
			public const string FIELD_COMMENT = "Comment";
			public const string FIELD_EXPIRES = "Expires";
			public const string FIELD_TRANSFERID = "TransferId";
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) { Refresh(id); }
		}

		protected override ISqlObjectTableSpec table { get { return TableSpec.instance; } }

		private int _postId;
		public int postId {
			get {
				this.LoadIfNotLoaded();
				return this._postId;
			}
		}
		public Post post {
			get {
				return Post.LoadById(this.postId);
			}
		}

		private int _ownerId;
		public int ownerId {
			get {
				this.LoadIfNotLoaded();
				return this._ownerId;
			}
		}
		public User owner {
			get {
				return User.LoadById(this.ownerId);
			}
		}

		private int _originalBoardId;
		public int originalBoardId {
			get {
				this.LoadIfNotLoaded();
				return this._originalBoardId;
			}
		}
		public Board originalBoard {
			get {
				return Board.LoadById(this.originalBoardId);
			}
		}

		private int _moderatorId;
		public int moderatorId {
			get {
				this.LoadIfNotLoaded();
				return this._moderatorId;
			}
		}
		public Account moderator {
			get {
				return Account.LoadById(this.moderatorId);
			}
		}

		private DateTime _punishmentDate;
		public DateTime punishmentDate {
			get {
				this.LoadIfNotLoaded();
				return this._punishmentDate;
			}
		}

		private int _punishmentTypeId;
		public int punishmentTypeId {
			get {
				this.LoadIfNotLoaded();
				return this._punishmentTypeId;
			}
		}
		public PunishmentType punishmentType {
			get {
				return PunishmentType.LoadById(this.punishmentTypeId);
			}
		}

		private bool _isWithdrawed;
		public bool isWithdrawed {
			get {
				this.LoadIfNotLoaded();
				return this._isWithdrawed;
			}
		}

		private string _comment;
		public string comment {
			get {
				this.LoadIfNotLoaded();
				return this._comment;
			}
		}

		private DateTime _expires;
		public DateTime expires {
			get {
				this.LoadIfNotLoaded();
				return this._expires;
			}
		}

		private int? _transferId;
		public int? transferId {
			get {
				this.LoadIfNotLoaded();
				return this._transferId;
			}
		}
		public PunishmentTransfer transfer {
			get {
				return PunishmentTransfer.LoadById(this.transferId.Value);
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._postId = int.Parse(data[TableSpec.FIELD_POSTID]);
			this._ownerId = int.Parse(data[TableSpec.FIELD_OWNERID]);
			this._originalBoardId = int.Parse(data[TableSpec.FIELD_ORIGINALBOARDID]);
			this._moderatorId = int.Parse(data[TableSpec.FIELD_MODERATORID]);
			this._punishmentDate = Util.ParseDateTimeFromTimestamp(data[TableSpec.FIELD_PUNISHMENTDATE]).Value;
			this._punishmentTypeId = int.Parse(data[TableSpec.FIELD_PUNISHMENTTYPE]);
			this._isWithdrawed = Util.string2bool(data[TableSpec.FIELD_ISWITHDRAWED]);
			this._comment = data[TableSpec.FIELD_COMMENT];
			this._expires = Util.ParseDateTimeFromTimestamp(data[TableSpec.FIELD_EXPIRES]).Value;
			this._transferId = Util.ParseInt(data[TableSpec.FIELD_TRANSFERID]);
		}

		public XElement exportToXml(UserContext context) {
			return new XElement("punishment",
				this.post.exportToXmlBase(context),
				new XElement("owner", this.owner.exportToXmlForViewing(context)),
				new XElement("originalBoard", this.originalBoard.exportToXmlSimple(context, Board.SubboardsOptions.None)),
				new XElement("moderator", this.moderator.user.exportToXmlForViewing(context)),
				new XElement("punishmentDate", this.punishmentDate.ToXml()),
				this.punishmentType.exportToXml(context),
				new XElement("isWithdrawed", this.isWithdrawed.ToPlainString()),
				new XElement("comment", this.comment),
				new XElement("expires", this.expires),
				this.transferId.HasValue ? this.transfer.exportToXml(context) : null
			);
		}

	}
}
