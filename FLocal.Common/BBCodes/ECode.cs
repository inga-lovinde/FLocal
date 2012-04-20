using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class ECode : BBCode {

		public ECode()
			: base("ecode") {
		}

		public override string Format(IPostParsingContext context, ITextFormatter formatter) {
			return this.GetInnerHTML(context, new BBCodeHtmlFormatter());
		}

	}
}
