using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class Code : BBCode {

		public Code()
			: base("code") {
		}

		public override string Format(ITextFormatter formatter) {
			return "<pre>" + System.Web.HttpUtility.HtmlEncode(this.InnerBBCode) + "</pre><br/>";
		}

	}
}
