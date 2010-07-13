using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Core.DB;
using FLocal.Core.DB.conditions;

namespace FLocal.Common.dataobjects {
	public class QuickLink : SqlObject<QuickLink> {

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "QuickLinks";
			public const string FIELD_ID = "Id";
			public const string FIELD_NAME = "Name";
			public const string FIELD_URL = "Url";
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) { Refresh(id); }
		}

		protected override ISqlObjectTableSpec table { get { return TableSpec.instance; } }

		private string _name;
		public string name {
			get {
				this.LoadIfNotLoaded();
				return this._name;
			}
		}

		private string _url;
		public string url {
			get {
				this.LoadIfNotLoaded();
				return this._url;
			}
		}

		private static Dictionary<string, int> name2id = new Dictionary<string,int>();
		public static QuickLink LoadByName(string _name) {
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
			return QuickLink.LoadById(name2id[name]);
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._name = data[TableSpec.FIELD_NAME];
			this._url = data[TableSpec.FIELD_URL];
		}

	}
}
