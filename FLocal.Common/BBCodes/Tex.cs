using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class Tex : BBCode {

		public Tex()
			: base("tex") {
		}

		public override string Format(IPostParsingContext context, ITextFormatter<IPostParsingContext> formatter) {
			string tex = this.InnerBBCode;
			var upload = helpers.TexProcessor.getCompiled(tex);
			return "<f:img><f:src>/Upload/Item/" + upload.id.ToString() + "/</f:src><f:alt>" + this.Safe(tex) + "</f:alt></f:img>";
		}

	}
}
