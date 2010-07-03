using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class FUrl : BBCode {

		public FUrl()
			: base("furl") {
		}

		public override string Format(ITextFormatter formatter) {
			string rawUrl = this.DefaultOrValue;
			Uri uri = new Uri(rawUrl);
			return "<a href=\"" + uri.ToString() + "\">" + this.GetInnerHTML(formatter) + "</a>";
		}

	}
}
