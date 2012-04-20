using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;
using System.Web;

namespace FLocal.Common.BBCodes {
	class Google : BBCode {

		public Google()
			: base("google") {
		}

		public override string Format(IPostParsingContext context, ITextFormatter<IPostParsingContext> formatter) {
			return "<a href=\"http://lmgtfy.com/?q=" + HttpUtility.UrlPathEncode(this.DefaultOrValue) + "\">g:" + this.GetInnerHTML(context, formatter) + "</a>";
		}

	}
}
