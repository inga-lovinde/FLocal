using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Core.DB.conditions {
	public class NotIsNullCondition : SimpleCondition {

		public readonly ColumnSpec column;

		public NotIsNullCondition(ColumnSpec column) {
			this.column = column;
		}

	}
}
