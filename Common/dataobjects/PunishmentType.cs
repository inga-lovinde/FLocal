using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Core.DB;
using FLocal.Core.DB.conditions;

namespace FLocal.Common.dataobjects {
	public class PunishmentType : SqlObject<PunishmentType> {

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "PunishmentTypes";
			public const string FIELD_ID = "Id";
			public const string FIELD_DESCRIPTION = "Description";
			public const string FIELD_WEIGHT = "Weight";
			public const string FIELD_WEIGHTDESCRIPTION = "WeightDescription";
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) { Refresh(id); }
		}

		protected override ISqlObjectTableSpec table { get { return TableSpec.instance; } }

		private string _description;
		public string description {
			get {
				this.LoadIfNotLoaded();
				return this._description;
			}
		}

		private int _weight;
		public int weight {
			get {
				this.LoadIfNotLoaded();
				return this._weight;
			}
		}

		private string _weightDescription;
		public string weightDescription {
			get {
				this.LoadIfNotLoaded();
				return this._weightDescription;
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._description = data[TableSpec.FIELD_DESCRIPTION];
			this._weight = int.Parse(data[TableSpec.FIELD_WEIGHT]);
			this._weightDescription = data[TableSpec.FIELD_WEIGHTDESCRIPTION];
		}

		public XElement exportToXml(UserContext context) {
			return new XElement("punishmentType",
				new XElement("id", this.id),
				new XElement("description", this.description),
				new XElement("weight", this.weight),
				new XElement("weightDescription", this.weightDescription)
			);
		}

		private static readonly object allTypes_Locker = new object();
		public static IEnumerable<PunishmentType> allTypes {
			get {
				return
					from id in Cache<IEnumerable<int>>.instance.get(
						allTypes_Locker,
						() => {
							IEnumerable<int> ids = (from stringId in Config.instance.mainConnection.LoadIdsByConditions(
								TableSpec.instance,
								new FLocal.Core.DB.conditions.EmptyCondition(),
								Diapasone.unlimited
							) select int.Parse(stringId)).ToList();
							PunishmentType.LoadByIds(ids);
							return ids;
						}
					)
					let type = PunishmentType.LoadById(id)
					orderby type.weight
					select type;
			}
		}
		internal static void allTypes_Reset() {
			Cache<IEnumerable<int>>.instance.delete(allTypes_Locker);
		}

	}
}
