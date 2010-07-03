using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using PJonDevelopment.BBCode;

namespace FLocal.Common {
	public static class UBBParser {

		private class BBParserGateway {

			public static readonly BBParserGateway instance = new BBParserGateway();

			private BBCodeParser parser;
			private ITextFormatter formatter;

			private BBParserGateway() {
				this.parser = new BBCodeParser();
				this.parser.ElementTypes.Add("b", typeof(BBCodes.B), true);
				this.parser.ElementTypes.Add("code", typeof(BBCodes.Code), true);
				this.parser.ElementTypes.Add("furl", typeof(BBCodes.FUrl), true);
				this.parser.ElementTypes.Add("i", typeof(BBCodes.I), true);
				this.parser.ElementTypes.Add("image", typeof(BBCodes.Image), true);
				this.parser.ElementTypes.Add("s", typeof(BBCodes.S), true);
				this.parser.ElementTypes.Add("u", typeof(BBCodes.U), true);
				this.parser.ElementTypes.Add("url", typeof(BBCodes.Url), true);
				this.formatter = new BBCodeHtmlFormatter();
			}

			public string Parse(string input) {
				return this.parser.Parse(input).Format(this.formatter);
			}

		}

		public static string UBBToIntermediate(string UBB) {
			//return HttpUtility.HtmlEncode(UBB).Replace("\r\n", "<br/>\r\n");
			return BBParserGateway.instance.Parse(UBB);
		}

		public static string ShallerToUBB(string shaller) {
			return shaller;
		}

	}
}
