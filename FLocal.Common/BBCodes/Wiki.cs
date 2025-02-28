﻿using System;
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

		public override string Format(IPostParsingContext context, ITextFormatter<IPostParsingContext> formatter) {
			return "<a href=\"http://en.wikipedia.org/wiki/" + HttpUtility.UrlPathEncode(this.DefaultOrValue) + "\">w:" + this.GetInnerHTML(context, formatter) + "</a>";
		}

	}
}
