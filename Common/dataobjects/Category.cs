using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.dataobjects {
	public class Category : SqlObject<Category> {

		private class TableSpec : FLocal.Core.DB.ITableSpec {
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return "Categories"; } }
			public string idName { get { return "Id"; } }
		}

		protected override FLocal.Core.DB.ITableSpec table { get { return TableSpec.instance; } }

		private string _name;
		public string name {
			get {
				this.LoadIfNotLoaded();
				return this._name;
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._name = data["Name"];
		}

	}
}
