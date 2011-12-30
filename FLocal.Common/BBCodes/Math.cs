using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class Math : BBCode {

		public Math()
			: base("math") {
		}

		public override string Format(ITextFormatter formatter) {
			string tex = "$$" + this.InnerBBCode + "$$";
			var upload = helpers.TexProcessor.getCompiled(tex);
			return "<f:img><f:src>/Upload/Item/" + upload.id.ToString() + "/</f:src><f:alt>" + this.Safe(tex) + "</f:alt></f:img>";
		}

	}
}
