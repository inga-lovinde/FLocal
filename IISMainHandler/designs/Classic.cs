using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.IISHandler.designs {
	class Classic : IDesign {

		public string GetFSName(string template) {
			return System.IO.Path.Combine("Full", template);
		}

		string FLocal.Common.IOutputParams.preprocessBodyIntermediate(string bodyIntermediate) {
			return bodyIntermediate.
				Replace("<f:img><f:src>", "<img src=\"").
				Replace("</f:src><f:alt>", "\" alt=\"").
				Replace("</f:alt></f:img>", "\"/>");
		}

		public string ContentType {
			get {
				return "text/html";
			}
		}

	}
}
