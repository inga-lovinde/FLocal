using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class Url : BBCode {

		public Url()
			: base("url") {
		}

		public override string Format(ITextFormatter formatter) {
			string rawUrl = this.Default;
			if(rawUrl == null) {
//				throw new ApplicationException(String.Join("; ", (from kvp in this.Attributes select kvp.Key + "=" + kvp.Value).ToArray()));
				rawUrl = this.InnerText;
			}
			var urlInfo = UrlProcessor.Process(rawUrl);
			return "<a href=\"" + urlInfo.relativeUrl + "\">" + this.GetInnerHTML(formatter) + "</a>";
		}

	}
}
