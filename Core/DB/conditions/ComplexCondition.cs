using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core.DB.conditions {
	class ComplexCondition : AbstractCondition {

		public readonly ConditionsJoinType type;

		public readonly List<AbstractCondition> innerConditions;

		public ComplexCondition(ConditionsJoinType type, List<AbstractCondition> innerConditions) {
			this.type = type;
			this.innerConditions = innerConditions;
		}

	}
}
