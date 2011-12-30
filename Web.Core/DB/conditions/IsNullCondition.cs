using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Core.DB.conditions {
	public class IsNullCondition : SimpleCondition {

		public readonly ColumnSpec column;

		public IsNullCondition(ColumnSpec column) {
			this.column = column;
		}

	}
}
