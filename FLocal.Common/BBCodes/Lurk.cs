using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;
using System.Web;

namespace FLocal.Common.BBCodes {
	class Lurk : BBCode {

		public Lurk()
			: base("lurk") {
		}

		public override string Format(IPostParsingContext context, ITextFormatter<IPostParsingContext> formatter) {
			return "<a href=\"http://lurkmore.ru/" + HttpUtility.UrlPathEncode(this.DefaultOrValue) + "\">l:" + this.GetInnerHTML(context, formatter) + "</a>";
		}

	}
}
