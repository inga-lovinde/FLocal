using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;
using FLocal.Common.URL;

namespace FLocal.Common.BBCodes {
	abstract class AbstractLocalLink : BBCode {

		protected AbstractLocalLink(string name)
			: base(name) {
		}

		abstract protected AbstractUrl url {
			get;
		}

		public override string Format(ITextFormatter formatter) {
			var url = this.url;
			var name = this.Safe(url.title);
			if(this.Default != null) {
				name = this.GetInnerHTML(formatter);
			}
			return string.Format("<a href=\"{0}\">{1}</a>", url.canonical, url.title);
		}

	}
}
