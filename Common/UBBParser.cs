using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FLocal.Common {
	public static class UBBParser {

		public static string UBBToIntermediate(string UBB) {
			return HttpUtility.HtmlEncode(UBB).Replace("\r\n", "<br/>\r\n");
		}

		public static string ShallerToUBB(string shaller) {
			return shaller;
		}

	}
}
