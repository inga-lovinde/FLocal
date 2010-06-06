using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core.DB;
using FLocal.Core.DB.conditions;

namespace FLocal.MySQLConnector {
	class ConditionCompiler {

		private readonly ParamsHolder paramsholder;

		private ConditionCompiler() {
			this.paramsholder = new ParamsHolder();
		}

		private string getName(ColumnOrValue cov) {
			if(cov.isColumn) {
				return cov.column.compile();
			} else {
				return "@" + this.paramsholder.Add(cov.value);
			}
		}

		private string CompileCondition(ComparisonCondition condition) {
			string left = condition.left.compile();
			string right = getName(condition.right);
			switch(condition.comparison) {
				case ComparisonType.EQUAL:
					return left + " = " + right;
				case ComparisonType.GREATEROREQUAL:
					return left + " >= " + right;
				case ComparisonType.GREATERTHAN:
					return left + " > " + right;
				case ComparisonType.LESSOREQUAL:
					return left + " <= " + right;
				case ComparisonType.LESSTHAN:
					return left + " < " + right;
				case ComparisonType.NOTEQUAL:
					return left + " != " + right;
				default:
					throw new NotImplementedException();
			}
		}

		private string CompileCondition(IsNullCondition condition) {
			return condition.column.compile() + " IS NULL";
		}

		private string CompileCondition(NotIsNullCondition condition) {
			return condition.column.compile() + " NOT IS NULL";
		}

		private string CompileCondition(MultiValueCondition condition) {
			List<string> valueParams = new List<string>();
			foreach(string value in condition.values) {
				valueParams.Add(this.paramsholder.Add(value));
			}

			if(condition.inclusive) {
				return condition.column.compile() + " IN (" + string.Join(", ", valueParams.ToArray()) + ")";
			} else {
				return condition.column.compile() + " IN (" + string.Join(", ", valueParams.ToArray()) + ")";
			}
		}

		private string CompileCondition(SimpleCondition condition) {
			if(condition is ComparisonCondition) {
				return CompileCondition((ComparisonCondition)condition);
			} else if(condition is IsNullCondition) {
				return CompileCondition((IsNullCondition)condition);
			} else if(condition is NotIsNullCondition) {
				return CompileCondition((NotIsNullCondition)condition);
			} else if(condition is MultiValueCondition) {
				return CompileCondition((MultiValueCondition)condition);
			} else {
				throw new NotImplementedException();
			}
		}

		private string CompileCondition(ComplexCondition condition) {
			List<string> parts = new List<string>();
			foreach(AbstractCondition innerCondition in condition.innerConditions) {
				parts.Add("(" + CompileCondition(innerCondition) + ")");
			}

			switch(condition.type) {
				case ConditionsJoinType.AND:
					return string.Join(" AND ", parts.ToArray());
				case ConditionsJoinType.OR:
					return string.Join(" OR ", parts.ToArray());
				default:
					throw new NotImplementedException();
			}
		}

		private string CompileCondition(AbstractCondition condition) {
			if(condition is ComplexCondition) {
				return CompileCondition((ComplexCondition)condition);
			} else if(condition is SimpleCondition) {
				return CompileCondition((SimpleCondition)condition);
			} else {
				throw new NotImplementedException();
			}
		}

		public static KeyValuePair<string, ParamsHolder> Compile(AbstractCondition condition) {
			ConditionCompiler compiler = new ConditionCompiler();
			string compiled = compiler.CompileCondition(condition);
			return new KeyValuePair<string,ParamsHolder>(compiled, compiler.paramsholder);
		}

	}
}
