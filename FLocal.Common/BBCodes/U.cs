﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class U : BBCode {

		public U()
			: base("u") {
		}

		public override string Format(IPostParsingContext context, ITextFormatter<IPostParsingContext> formatter) {
			return "<u>" + this.GetInnerHTML(context, formatter) + "</u>";
		}

	}
}
