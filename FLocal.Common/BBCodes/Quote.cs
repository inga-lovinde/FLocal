using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Core;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class Quote : BBCode {

		public Quote()
			: base("quote") {
		}

		public override string Format(IPostParsingContext context, ITextFormatter<IPostParsingContext> formatter) {
			string inner = this.GetInnerHTML(context, formatter).TrimHtml();
			if(inner == "") return "";
			string marker = this.Default;
			if(marker == null) marker = "Quote:";
			return "<blockquote><div class=\"quotetitle\">" + marker + "</div><div class=\"quotecontent\">" + inner + "</div></blockquote>";
		}

	}
}
