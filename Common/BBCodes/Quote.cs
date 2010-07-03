using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class Quote : BBCode {

		public Quote()
			: base("quote") {
		}

		public override string Format(ITextFormatter formatter) {
			string marker = this.Default;
			if(marker == null) marker = "Quote:";
			return "<blockquote><font class=\"small\">" + marker + "</font><hr/><br/>" + this.GetInnerHTML(formatter).Trim() + "<br/><br/><hr/></blockquote><br/>";
		}

	}
}
