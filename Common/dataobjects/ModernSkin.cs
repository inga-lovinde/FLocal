using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Web.Core;
using Web.Core.DB;
using Web.Core.DB.conditions;

namespace FLocal.Common.dataobjects {
	public class ModernSkin : SqlObject<ModernSkin> {

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "ModernSkins";
			public const string FIELD_ID = "Id";
			public const string FIELD_NAME = "SkinName";
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

		protected override void doFromHash(Dictionary<string, string> data) {
			this._name = data[TableSpec.FIELD_NAME];
		}

		private static readonly object allSkins_Locker = new object();
		public static IEnumerable<ModernSkin> allSkins {
			get {
				return
					from id in Cache<List<int>>.instance.get(
						allSkins_Locker,
						() => {
							List<int> ids = (from stringId in Config.instance.mainConnection.LoadIdsByConditions(
								TableSpec.instance,
								new Web.Core.DB.conditions.EmptyCondition(),
								Diapasone.unlimited
							) select int.Parse(stringId)).ToList();
							ModernSkin.LoadByIds(ids);
							return ids;
						}
					)
					let skin = ModernSkin.LoadById(id)
					orderby skin.id
					select skin;
			}
		}
		internal static void allSkins_Reset() {
			Cache<IEnumerable<int>>.instance.delete(allSkins_Locker);
		}

		public XElement exportToXml() {
			return new XElement("modernSkin",
				new XElement("id", this.id),
				new XElement("name", this.name)
			);
		}

		private static Dictionary<string, int> name2id = new Dictionary<string, int>();
		public static ModernSkin LoadByName(string _name) {
			string name = _name;
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
			return ModernSkin.LoadById(name2id[name]);
		}

	}
}
