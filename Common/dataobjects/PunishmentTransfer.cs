using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Core.DB;
using FLocal.Core.DB.conditions;

namespace FLocal.Common.dataobjects {
	public class PunishmentTransfer : SqlObject<PunishmentTransfer> {

		public struct NewTransferInfo {

			public readonly int newBoardId;
			public Board newBoard {
				get {
					return Board.LoadById(this.newBoardId);
				}
			}

			public readonly bool isSubthreadTransfer;

			public NewTransferInfo(Board newBoard, bool isSubthreadTransfer) {
				this.newBoardId = newBoard.id;
				this.isSubthreadTransfer = isSubthreadTransfer;
			}

		}

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "PunishmentTransfers";
			public const string FIELD_ID = "Id";
			public const string FIELD_OLDBOARDID = "OldBoardId";
			public const string FIELD_OLDPARENTPOSTID = "OldParentPostId";
			public const string FIELD_NEWBOARDID = "NewBoardId";
			public const string FIELD_ISSUBTHREADTRANSFER = "IsSubthreadMove";
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) { Refresh(id); }
		}

		protected override ISqlObjectTableSpec table { get { return TableSpec.instance; } }

		private int _oldBoardId;
		public int oldBoardId {
			get {
				this.LoadIfNotLoaded();
				return this._oldBoardId;
			}
		}
		public Board oldBoard {
			get {
				return Board.LoadById(this.oldBoardId);
			}
		}

		private int? _oldParentPostId;
		public int? oldParentPostId {
			get {
				this.LoadIfNotLoaded();
				return this._oldParentPostId;
			}
		}
		public Post oldParentPost {
			get {
				return Post.LoadById(this.oldParentPostId.Value);
			}
		}

		private int _newBoardId;
		public int newBoardId {
			get {
				this.LoadIfNotLoaded();
				return this._newBoardId;
			}
		}
		public Board newBoard {
			get {
				return Board.LoadById(this.newBoardId);
			}
		}

		private bool _isSubthreadTransfer;
		public bool isSubthreadTransfer {
			get {
				this.LoadIfNotLoaded();
				return this._isSubthreadTransfer;
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._oldBoardId = int.Parse(data[TableSpec.FIELD_OLDBOARDID]);
			this._oldParentPostId = Util.ParseInt(data[TableSpec.FIELD_OLDPARENTPOSTID]);
			this._newBoardId = int.Parse(data[TableSpec.FIELD_NEWBOARDID]);
			this._isSubthreadTransfer = Util.string2bool(data[TableSpec.FIELD_ISSUBTHREADTRANSFER]);
		}

		public XElement exportToXml(UserContext context) {
			return new XElement("transfer",
				new XElement("id", this.id),
				new XElement("oldBoard", this.oldBoard.exportToXmlSimple(context)),
				this.oldParentPostId.HasValue ? new XElement("oldParentPost", this.oldParentPost.exportToXmlBase(context)) : null,
				new XElement("newBoard", this.newBoard.exportToXmlSimple(context)),
				new XElement("isSubthreadTransfer", this.isSubthreadTransfer.ToPlainString())
			);
		}

	}
}
