using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class Font : BBCode {

		public Font()
			: base("font") {
		}

		public override string Format(IPostParsingContext context, ITextFormatter<IPostParsingContext> formatter) {
			return "<font face=\"" + this.Default + "\">" + this.GetInnerHTML(context, formatter) + "</font>";
		}

	}
}
