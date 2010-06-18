using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.actions {
	public class ScalarFieldValue : AbstractFieldValue {

		private readonly string scalar;

		public ScalarFieldValue(string scalar) {
			this.scalar = scalar;
		}

		public override string getStringRepresentation() {
			return this.scalar;
		}
		public override string getStringRepresentation(string oldInfo) {
			return this.scalar;
		}

	}
}
