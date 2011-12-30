using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class I : BBCode {

		public I()
			: base("i") {
		}

		public override string Format(ITextFormatter formatter) {
			return "<i>" + this.GetInnerHTML(formatter) + "</i>";
		}

	}
}
