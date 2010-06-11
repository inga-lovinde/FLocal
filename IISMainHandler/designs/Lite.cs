using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.IISHandler.designs {
	class Lite : IDesign {
		public string fsname {
			get {
				return "Lite";
			}
		}

		string FLocal.Common.IOutputParams.preprocessBodyIntermediate(string bodyIntermediate) {
			return bodyIntermediate.
				Replace("<f:img><f:src>", "<a href=\"").
				Replace("</f:src><f:alt>", "\">").
				Replace("</f:alt></f:img>", "</a>");
		}
	}
}
