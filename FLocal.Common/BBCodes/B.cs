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

		public override string Format(IPostParsingContext context, ITextFormatter formatter) {
			return "<b>" + this.GetInnerHTML(context, formatter) + "</b>";
		}

	}
}
