using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core.DB {
	class SortSpec {

		public readonly ColumnSpec column;
	
		public readonly bool ascending;

		public SortSpec(ColumnSpec column, bool ascending) {
			this.column = column;
			this.ascending = ascending;
		}

	}
}
