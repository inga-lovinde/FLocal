using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.BBCodes {
	abstract class BBCode : PJonDevelopment.BBCode.BBCodeElement {

		public BBCode(string name)
			: base(name) {
		}

		protected string GetInnerHTML(PJonDevelopment.BBCode.ITextFormatter formatter) {
			StringBuilder builder = new StringBuilder();
			foreach (var node in this.Nodes) {
				builder.Append(node.Format(formatter));
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

	}
}
