﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class Quote : BBCode {

		public Quote()
			: base("quote") {
		}

		public override string Format(ITextFormatter formatter) {
			string inner = this.GetInnerHTML(formatter).TrimHtml();
			if(inner == "") return "";
			string marker = this.Default;
			if(marker == null) marker = "Quote:";
			return "<br/><blockquote><font class=\"small\">" + marker + "</font><hr/>" + inner + "<br/><hr/></blockquote><br/>";
		}

	}
}
