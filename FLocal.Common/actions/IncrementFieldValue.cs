using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Core;

namespace FLocal.Common.actions {
	class IncrementFieldValue : AbstractFieldValue {

		public static Func<string, string> INCREMENTOR_CUSTOM(int i) {
			return s => {
				int old = String.IsNullOrEmpty(s) ? 0 : int.Parse(s);
				return (old+i).ToString();
			};
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

		private readonly Box<string> resultBox;

		public IncrementFieldValue(Func<string, string> processor) {
			this.processor = processor;
		}

		public IncrementFieldValue(Func<string, string> processor, Box<string> resultBox) : this(processor) {
			this.resultBox = resultBox;
		}

		public IncrementFieldValue() : this(INCREMENTOR) {
		}

		public override string getStringRepresentation() {
			throw new NotSupportedException();
		}
		public override string getStringRepresentation(string oldInfo) {
			var result = this.processor(oldInfo);
			if(this.resultBox != null) {
				this.resultBox.value = result;
			}
			return result;
		}

	}
}
