using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Web.Core;
using Web.Core.DB;
using Web.Core.DB.conditions;
using Web.Core.Network;

namespace FLocal.Common.dataobjects {
	public class LocalNetwork : SqlObject<LocalNetwork> {

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "LocalNetworks";
			public const string FIELD_ID = "Id";
			public const string FIELD_NETWORK = "Network";
			public const string FIELD_COMMENT = "Comment";
			public const string FIELD_ISENABLED = "IsEnabled";
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) { Refresh(id); }
		}

		protected override ISqlObjectTableSpec table { get { return TableSpec.instance; } }

		private IPv4Subnet _network;
		public IPv4Subnet network {
			get {
				this.LoadIfNotLoaded();
				return this._network;
			}
		}

		private string _comment;
		public string comment {
			get {
				this.LoadIfNotLoaded();
				return this._comment;
			}
		}

		private bool _isEnabled;
		public bool isEnabled {
			get {
				this.LoadIfNotLoaded();
				return this._isEnabled;
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._network = IPv4Subnet.FromString(data[TableSpec.FIELD_NETWORK]);
			this._comment = data[TableSpec.FIELD_COMMENT];
			this._isEnabled = Util.string2bool(data[TableSpec.FIELD_ISENABLED]);
		}

		public XElement exportToXml(UserContext context) {
			return new XElement("localNetwork",
				new XElement("id", this.id),
				new XElement("network", this.network.ToString()),
				new XElement("comment", this.comment),
				new XElement("isEnabled", this.isEnabled.ToPlainString())
			);
		}

		public static bool IsLocalNetwork(IPv4Address ip) {
			return Config.instance.mainConnection.GetCountByConditions(
				TableSpec.instance,
				new ComplexCondition(
					ConditionsJoinType.AND,
					new ComparisonCondition(
						TableSpec.instance.getColumnSpec(TableSpec.FIELD_ISENABLED),
						ComparisonType.EQUAL,
						"1"
					),
					new MultiValueCondition(
						TableSpec.instance.getColumnSpec(TableSpec.FIELD_NETWORK),
						(from subnet in ip.matchingSubnets select subnet.ToString()).ToArray()
					)
				)
			) > 0;
		}

	}
}
