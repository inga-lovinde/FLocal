using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FLocal.IISHandler {
	static class Extensions {

		public static void WriteLine(this HttpResponse response, string toWrite) {
			response.Write(toWrite);
			response.Write(Core.Util.EOL);
		}

		public static string[] Split(this string str, string separator, StringSplitOptions options) {
			return str.Split(new string[] { separator }, options);
		}

	}
}
