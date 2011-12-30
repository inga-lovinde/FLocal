using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Core.DB {
	public class ColumnSpec {

		public readonly ITableSpec table;

		public readonly string name;

		public ColumnSpec(ITableSpec table, string name) {
			this.table = table;
			this.name = name;
		}

	}
}
