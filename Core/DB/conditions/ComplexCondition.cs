using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core.DB.conditions {
	public class ComplexCondition : NotEmptyCondition {

		public readonly ConditionsJoinType type;

		public readonly List<NotEmptyCondition> innerConditions;

		public ComplexCondition(ConditionsJoinType type, List<NotEmptyCondition> innerConditions) {
			this.type = type;
			this.innerConditions = innerConditions;
		}

	}
}
