using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using FLocal.Core;

namespace FLocal.IISHandler {
	static class Extensions {

		public static void WriteLine(this HttpResponse response, string toWrite) {
			response.Write(toWrite);
			response.Write(Core.Util.EOL);
		}

		public static string[] Split(this string str, string separator, StringSplitOptions options) {
			return str.Split(new string[] { separator }, options);
		}

		public static IEnumerable<XElement> addNumbers(this IEnumerable<XElement> elements) {
			int i=0;
			foreach(XElement element in elements) {
				XElement result = new XElement(element);
				result.Add(new XElement("number", i));
				result.Add(new XElement("even", (i%2 == 0).ToPlainString()));
				i++;
				yield return result;
			}
		}

	}
}
