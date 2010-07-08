using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core.DB {
	public class JoinSpec {

		private class TableSpec : ITableSpec {
			private readonly string _name;
			public TableSpec(string name) {
				this._name = name;
			}
			public string name {
				get {
					return this._name;
				}
			}
			public string idName {
				get {
					throw new NotImplementedException();
				}
			}
		}

		public readonly ColumnSpec mainColumn;

		public readonly ITableSpec additionalTable;
		public readonly ITableSpec additionalTableJoin;

		public JoinSpec(ColumnSpec mainColumn, ITableSpec additionalTable, string alias) {
			this.mainColumn = mainColumn;
			this.additionalTableJoin = additionalTable;
			this.additionalTable = new TableSpec(alias);
		}

	}
}
