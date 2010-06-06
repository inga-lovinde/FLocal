using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core.DB.conditions {
	public class MultiValueCondition : SimpleCondition {

		public readonly ColumnSpec column;

		public readonly bool inclusive;

		public readonly string[] values;

		public MultiValueCondition(ColumnSpec column, string[] values, bool inclusive) {
			this.column = column;
			this.values = values;
			this.inclusive = inclusive;
		}

		public MultiValueCondition(ColumnSpec column, string[] values) : this(column, values, true) {
		}

	}
}
