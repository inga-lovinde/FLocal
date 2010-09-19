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

		public override string Format(ITextFormatter formatter) {
			return this.GetInnerHTML(new BBCodeHtmlFormatter());
		}

	}
}
