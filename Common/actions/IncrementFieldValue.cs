using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.actions {
	class IncrementFieldValue : AbstractFieldValue {

		public static Func<string, string> INCREMENTOR_CUSTOM(int i) {
			return s => (int.Parse(s)+i).ToString();
		}

		public static Func<string, string> DECREMENTOR_CUSTOM(int i) {
			return s => (int.Parse(s)-i).ToString();
		}

		public static string INCREMENTOR(string s) {
			return INCREMENTOR_CUSTOM(1)(s);
		}

		public static string DECREMENTOR(string s) {
			return DECREMENTOR_CUSTOM(1)(s);
		}

		public static Func<string, string> GREATEST(int val) {
			return s => {
				if(s == null || s == "") {
					return val.ToString();
				} else {
					return Math.Max(int.Parse(s), val).ToString();
				}
			};
		}

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
