using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class List : BBCode {

		public List()
			: base("list") {
		}

		public override string Format(ITextFormatter formatter) {
			return "<ul>" + this.GetInnerHTML(formatter) + "</ul>";
		}

	}
}
