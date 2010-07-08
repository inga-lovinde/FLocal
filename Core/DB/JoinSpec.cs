using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core.DB {
	public class JoinSpec {

		private class TableSpec : ITableSpec {
			private readonly string _name;
			private readonly string _idName;
			public TableSpec(ITableSpec raw, string name) {
				this._name = name;
				this._idName = raw.idName;
			}
			public string name {
				get {
					return this._name;
				}
			}
			public string idName {
				get {
					return this._idName;
				}
			}
		}

		public readonly ColumnSpec mainColumn;

		public readonly ITableSpec additionalTable;
		public readonly ITableSpec additionalTableRaw;

		public JoinSpec(ColumnSpec mainColumn, ITableSpec additionalTable, string alias) {
			this.mainColumn = mainColumn;
			this.additionalTableRaw = additionalTable;
			this.additionalTable = new TableSpec(additionalTable, alias);
		}

	}
}
