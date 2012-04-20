using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class Image : BBCode {

		public Image() : base("image") {
		}

		public override string Format(IPostParsingContext context, ITextFormatter<IPostParsingContext> formatter) {
			var urlInfo = UrlProcessor.Process(this.InnerText);
			if (urlInfo.isLocal && urlInfo.relativeUrl.StartsWith("/user/upload/")) {
				return "<f:img><f:src>" + urlInfo.relativeUrl + "</f:src><f:alt>" + urlInfo.relativeUrl + "</f:alt></f:img>";
			} else {
				return "<a href=\"" + urlInfo.relativeUrl + "\">" + urlInfo.relativeUrl + "</a>";
			}
		}

	}
}
