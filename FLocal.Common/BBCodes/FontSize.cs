using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class FontSize : BBCode {

		public FontSize()
			: base("size") {
		}

		public override string Format(ITextFormatter formatter) {
			return "<font size=\"" + this.Default + "\">" + this.GetInnerHTML(formatter) + "</font>";
		}

	}
}
