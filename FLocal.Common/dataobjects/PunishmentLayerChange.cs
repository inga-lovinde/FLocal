using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Web.Core;
using Web.Core.DB;
using Web.Core.DB.conditions;

namespace FLocal.Common.dataobjects {
	public class PunishmentLayerChange : SqlObject<PunishmentLayerChange> {

		public struct NewLayerChangeInfo {

			public readonly int newLayerId;
			public PostLayer newLayer {
				get {
					return PostLayer.LoadById(this.newLayerId);
				}
			}

			public readonly bool isSubthreadChange;

			public NewLayerChangeInfo(PostLayer newLayer, bool isSubthreadChange) {
				this.newLayerId = newLayer.id;
				this.isSubthreadChange = isSubthreadChange;
			}

		}

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "PunishmentLayerChanges";
			public const string FIELD_ID = "Id";
			public const string FIELD_OLDLAYERID = "OldLayerId";
			public const string FIELD_NEWLAYERID = "NewLayerId";
			public const string FIELD_ISSUBTHREADCHANGE = "IsSubthreadChange";
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) { Refresh(id); }
		}

		protected override ISqlObjectTableSpec table { get { return TableSpec.instance; } }

		private int _oldLayerId;
		public int oldLayerId {
			get {
				this.LoadIfNotLoaded();
				return this._oldLayerId;
			}
		}
		public PostLayer oldLayer {
			get {
				return PostLayer.LoadById(this.oldLayerId);
			}
		}

		private int _newLayerId;
		public int newLayerId {
			get {
				this.LoadIfNotLoaded();
				return this._newLayerId;
			}
		}
		public PostLayer newLayer {
			get {
				return PostLayer.LoadById(this.newLayerId);
			}
		}


		private bool _isSubthreadChange;
		public bool isSubthreadChange {
			get {
				this.LoadIfNotLoaded();
				return this._isSubthreadChange;
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._oldLayerId = int.Parse(data[TableSpec.FIELD_OLDLAYERID]);
			this._newLayerId = int.Parse(data[TableSpec.FIELD_NEWLAYERID]);
			this._isSubthreadChange = Util.string2bool(data[TableSpec.FIELD_ISSUBTHREADCHANGE]);
		}

		public XElement exportToXml(UserContext context) {
			return new XElement("layerChange",
				new XElement("id", this.id),
				new XElement("oldLayer", this.oldLayer.exportToXml(context)),
				new XElement("newLayer", this.newLayer.exportToXml(context)),
				new XElement("isSubthreadChange", this.isSubthreadChange.ToPlainString())
			);
		}

	}
}
