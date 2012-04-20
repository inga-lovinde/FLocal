using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Common.helpers;

namespace FLocal.Common.BBCodes {
	abstract class BBCode : PJonDevelopment.BBCode.BBCodeElement<IPostParsingContext> {

		public BBCode(string name)
			: base(name) {
		}

		protected string GetInnerHTML(IPostParsingContext context, PJonDevelopment.BBCode.ITextFormatter<IPostParsingContext> formatter) {
			StringBuilder builder = new StringBuilder();
			foreach (var node in this.Nodes) {
				builder.Append(node.Format(context, formatter));
			}
			return builder.ToString();
		}

		protected string Default {
			get {
				if(!this.Attributes.ContainsKey("DEFAULT")) {
					return null;
				}
				string result = this.Attributes["DEFAULT"];
				if(result == null || result == "") {
					return null;
				}
				return result;
			}
		}

		protected string DefaultOrValue {
			get {
				string result = this.Default;
				if(result == null) {
					result = this.InnerText;
				}
				return result;
			}
		}

		protected string Safe(string str) {
			return System.Web.HttpUtility.HtmlEncode(str);
		}

	}
}
