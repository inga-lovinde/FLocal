using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core.DB.conditions {
	public class ComplexCondition : NotEmptyCondition {

		public readonly ConditionsJoinType type;

		public readonly NotEmptyCondition[] innerConditions;

		public ComplexCondition(ConditionsJoinType type, params NotEmptyCondition[] innerConditions) {
			this.type = type;
			this.innerConditions = innerConditions;
		}

	}
}
