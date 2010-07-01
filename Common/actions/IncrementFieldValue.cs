using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.actions {
	class IncrementFieldValue : AbstractFieldValue {

		public static readonly Func<string, string> INCREMENTOR = s => (int.Parse(s)+1).ToString();
		public static readonly Func<string, string> DECREMENTOR = s => (int.Parse(s)-1).ToString();

		private readonly Func<string, string> processor;

		public IncrementFieldValue(Func<string, string> processor) {
			this.processor = processor;
		}

		public IncrementFieldValue() : this(INCREMENTOR) {
		}

		public override string getStringRepresentation() {
			throw new NotSupportedException();
		}
		public override string getStringRepresentation(string oldInfo) {
			return this.processor(oldInfo);
		}

	}
}
