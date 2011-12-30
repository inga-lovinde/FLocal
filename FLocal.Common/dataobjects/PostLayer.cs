using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Web.Core;
using Web.Core.DB;
using Web.Core.DB.conditions;

namespace FLocal.Common.dataobjects {
	public class PostLayer : SqlObject<PostLayer> {

		public const string NAME_NORMAL = "normal";
		public const string NAME_OFFTOP = "offtop";
		public const string NAME_GARBAGE = "garbage";
		public const string NAME_HIDDEN = "hidden";

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "Layers";
			public const string FIELD_ID = "Id";
			public const string FIELD_NAME = "Name";
			public const string FIELD_MAXPUNISHMENTS = "MaxPunishments";
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

		private int? _maxPunishments;
		public int? maxPunishments {
			get {
				this.LoadIfNotLoaded();
				return this._maxPunishments;
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._name = data[TableSpec.FIELD_NAME];
			this._maxPunishments = Util.ParseInt(data[TableSpec.FIELD_MAXPUNISHMENTS]);
		}

		private static readonly object allLayers_Locker = new object();
		public static IEnumerable<PostLayer> allLayers {
			get {
				return
					from id in Cache<List<int>>.instance.get(
						allLayers_Locker,
						() => {
							List<int> ids = (from stringId in Config.instance.mainConnection.LoadIdsByConditions(
								TableSpec.instance,
								new Web.Core.DB.conditions.EmptyCondition(),
								Diapasone.unlimited
							) select int.Parse(stringId)).ToList();
							PostLayer.LoadByIds(ids);
							return ids;
						}
					)
					let layer = PostLayer.LoadById(id)
					orderby layer.id
					select layer;
			}
		}
		internal static void allLayers_Reset() {
			Cache<IEnumerable<int>>.instance.delete(allLayers_Locker);
		}

		public XElement exportToXml(UserContext context, params XElement[] additional) {
			XElement result = new XElement("layer",
				new XElement("id", this.id),
				new XElement("name", this.name)
			);
			if(additional.Length > 0) {
				result.Add(additional);
			}
			return result;
		}

	}
}
