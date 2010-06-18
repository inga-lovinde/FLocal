using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.actions {
	public class ReferenceFieldValue : AbstractFieldValue {

		public readonly AbstractChange referenced;

		public ReferenceFieldValue(AbstractChange referenced) {
			this.referenced = referenced;
		}

		public override string getStringRepresentation() {
			return this.referenced.getId().Value.ToString();
		}
		public override string getStringRepresentation(string oldInfo) {
			return this.referenced.getId().Value.ToString();
		}

	}
}
