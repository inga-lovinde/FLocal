using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;
using System.Web;

namespace FLocal.Common.BBCodes {
	class Wiki : BBCode {

		public Wiki()
			: base("wiki") {
		}

		public override string Format(ITextFormatter formatter) {
			return "<a href=\"http://en.wikipedia.org/wiki/" + HttpUtility.UrlEncode(this.DefaultOrValue) + "\">" + this.GetInnerHTML(formatter) + "</a>";
		}

	}
}
