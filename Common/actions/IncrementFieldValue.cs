using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.actions {
	class IncrementFieldValue : AbstractFieldValue {

		private readonly Func<string, string> incrementor;

		public IncrementFieldValue(Func<string, string> incrementor) {
			this.incrementor = incrementor;
		}

		public IncrementFieldValue()
			: this(str => (int.Parse(str)+1).ToString()) {
		}

		public override string getStringRepresentation() {
			throw new NotSupportedException();
		}
		public override string getStringRepresentation(string oldInfo) {
			return this.incrementor(oldInfo);
		}

	}
}
