using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class ListElem : BBCode {

		public ListElem()
			: base("*") {
		}

		public override string Format(IPostParsingContext context, ITextFormatter formatter) {
//			return "<li>" + this.GetInnerHTML(formatter) + "</li>";
			return "<li>";
		}

	}
}
