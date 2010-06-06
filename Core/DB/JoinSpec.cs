using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core.DB {
	public class JoinSpec {

		public readonly ColumnSpec mainColumn;

		public readonly ITableSpec additionalTable;

		public JoinSpec(ColumnSpec mainColumn, ITableSpec additionalTable) {
			this.mainColumn = mainColumn;
			this.additionalTable = additionalTable;
		}

	}
}
