using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class QuoteSkipper : BBCode {

		public QuoteSkipper() : base("quoteskipper") {
		}

		public override string Format(IPostParsingContext context, ITextFormatter formatter) {
			if(this.Name.ToLower() == "q" || this.Name.ToLower() == "quote") {
				return "";
			} else if(this.Name.ToLower() == "code") {
				return "[code]" + this.InnerBBCode + "[/code]";
			} else {
				string name = this.Name;
				if(name.ToLower() == "uploadimage") name = "uploadLink";
				var sb = new StringBuilder();
				sb.Append("[");
				sb.Append(name);
				if(this.Default != null && this.Default != "") {
					sb.Append("='");
					sb.Append(this.Default.Replace("'", "''"));
					sb.Append("'");
				} else {
					foreach(var attribute in this.Attributes) {
						sb.Append(" ");
						sb.Append(attribute.Key);
						sb.Append("='");
						sb.Append(attribute.Value.Replace("'", "''"));
						sb.Append("'");
					}
				}
				sb.Append("]");
				if(this.RequireClosingTag) {
					sb.Append(this.GetInnerHTML(context, formatter));
					sb.Append("[/");
					sb.Append(name);
					sb.Append("]");
				}
				return sb.ToString();
			}
		}

	}
}
