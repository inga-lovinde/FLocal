using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class B : BBCode {

		public B()
			: base("b") {
		}

		public override string Format(ITextFormatter formatter) {
			return "<b>" + this.GetInnerHTML(formatter) + "</b>";
		}

	}
}
