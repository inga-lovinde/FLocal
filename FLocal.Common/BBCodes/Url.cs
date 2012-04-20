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

		public override string Format(IPostParsingContext context, ITextFormatter<IPostParsingContext> formatter) {
			string rawUrl = this.DefaultOrValue;
			string title = null;
			if(rawUrl.ToLower() != this.InnerText.ToLower()) {
				title = this.GetInnerHTML(context, formatter);
			}
			return UrlProcessor.ProcessLink(rawUrl, title, true);
		}

	}
}
