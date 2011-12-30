using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.actions {

	class TwoWayReferenceFieldValue : ReferenceFieldValue {

		public delegate string Calculator(string old, string reference);

		public static Calculator GREATEST = (old, reference) => {
			if(old == null || old == "") {
				return reference;
			}
			return Math.Max(int.Parse(old), int.Parse(reference)).ToString();
		};

		private Calculator calculator;

		public TwoWayReferenceFieldValue(AbstractChange referenced, Calculator calculator)
			: base(referenced) {
			this.calculator = calculator;
		}

		public override string getStringRepresentation() {
			throw new NotSupportedException();
		}

		public override string getStringRepresentation(string oldInfo) {
			return this.calculator(oldInfo, base.getStringRepresentation());
		}

	}
}
