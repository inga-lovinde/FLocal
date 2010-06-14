using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core.DB;
using FLocal.Core.DB.conditions;

namespace FLocal.MySQLConnector {
	class ConditionCompiler {

		private readonly ParamsHolder paramsholder;
		private readonly IDBTraits traits;

		private ConditionCompiler(IDBTraits traits) {
			this.paramsholder = new ParamsHolder();
			this.traits = traits;
		}

		private string getName(ColumnOrValue cov) {
			if(cov.isColumn) {
				return cov.column.compile(this.traits);
			} else {
				return this.traits.markParam(this.paramsholder.Add(cov.value));
			}
		}

		private string CompileCondition(ComparisonCondition condition) {
			string left = condition.left.compile(this.traits);
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
					throw new NotSupportedException();
			}
		}

		private string CompileCondition(IsNullCondition condition) {
			return condition.column.compile(this.traits) + " IS NULL";
		}

		private string CompileCondition(NotIsNullCondition condition) {
			return condition.column.compile(this.traits) + " NOT IS NULL";
		}

		private string CompileCondition(MultiValueCondition condition) {
			List<string> valueParams = new List<string>();
			foreach(string value in condition.values) {
				valueParams.Add(this.paramsholder.Add(value));
			}

			if(condition.inclusive) {
				return condition.column.compile(this.traits) + " IN (" + string.Join(", ", valueParams.ToArray()) + ")";
			} else {
				return condition.column.compile(this.traits) + " IN (" + string.Join(", ", valueParams.ToArray()) + ")";
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
				throw new NotSupportedException();
			}
		}

		private string CompileCondition(ComplexCondition condition) {
			List<string> parts = new List<string>();
			foreach(NotEmptyCondition innerCondition in condition.innerConditions) {
				parts.Add("(" + CompileCondition(innerCondition) + ")");
			}

			switch(condition.type) {
				case ConditionsJoinType.AND:
					return string.Join(" AND ", parts.ToArray());
				case ConditionsJoinType.OR:
					return string.Join(" OR ", parts.ToArray());
				default:
					throw new NotSupportedException();
			}
		}

		private string CompileCondition(NotEmptyCondition condition) {
			if(condition is ComplexCondition) {
				return CompileCondition((ComplexCondition)condition);
			} else if(condition is SimpleCondition) {
				return CompileCondition((SimpleCondition)condition);
			} else {
				throw new NotSupportedException();
			}
		}

		private string CompileCondition(EmptyCondition condition) {
			return "";
		}

		private string CompileCondition(AbstractCondition condition) {
			if(condition is NotEmptyCondition) {
				return CompileCondition((NotEmptyCondition)condition);
			} else if(condition is EmptyCondition) {
				return CompileCondition((EmptyCondition)condition);
			} else {
				throw new NotSupportedException();
			}
		}

		public static KeyValuePair<string, ParamsHolder> Compile(AbstractCondition condition, IDBTraits traits) {
			ConditionCompiler compiler = new ConditionCompiler(traits);
			string compiled = compiler.CompileCondition(condition);
			return new KeyValuePair<string,ParamsHolder>(compiled, compiler.paramsholder);
		}

	}
}
