using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class FontColor : BBCode {

		public FontColor()
			: base("color") {
		}

		public override string Format(IPostParsingContext context, ITextFormatter formatter) {
			return "<font color=\"" + this.Default + "\">" + this.GetInnerHTML(context, formatter) + "</font>";
		}

	}
}
