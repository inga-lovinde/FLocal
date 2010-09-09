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

		public override string Format(ITextFormatter formatter) {
			return "<a href=\"http://lmgtfy.com/?q=" + HttpUtility.UrlEncode(this.DefaultOrValue) + "\">g:" + this.GetInnerHTML(formatter) + "</a>";
		}

	}
}
