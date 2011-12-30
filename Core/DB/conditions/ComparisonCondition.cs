using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Core.DB.conditions {
	public class ComparisonCondition : SimpleCondition {

		public readonly ColumnSpec left;
		public readonly ComparisonType comparison;
		public readonly ColumnOrValue right;

		public ComparisonCondition(ColumnSpec left, ComparisonType comparison, ColumnSpec right) {
			this.left = left;
			this.comparison = comparison;
			this.right = ColumnOrValue.createColumn(right);
		}

		public ComparisonCondition(ColumnSpec left, ComparisonType comparison, string right) {
			this.left = left;
			this.comparison = comparison;
			this.right = ColumnOrValue.createValue(right);
		}

	}
}
