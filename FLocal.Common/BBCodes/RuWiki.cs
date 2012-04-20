using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;
using System.Web;

namespace FLocal.Common.BBCodes {
	class RuWiki : BBCode {

		public RuWiki()
			: base("ruwiki") {
		}

		public override string Format(IPostParsingContext context, ITextFormatter<IPostParsingContext> formatter) {
			return "<a href=\"http://ru.wikipedia.org/wiki/" + HttpUtility.UrlPathEncode(this.DefaultOrValue) + "\">в:" + this.GetInnerHTML(context, formatter) + "</a>";
		}

	}
}
