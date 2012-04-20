using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class S : BBCode {

		public S()
			: base("s") {
		}

		public override string Format(IPostParsingContext context, ITextFormatter<IPostParsingContext> formatter) {
			return "<s>" + this.GetInnerHTML(context, formatter) + "</s>";
		}

	}
}
