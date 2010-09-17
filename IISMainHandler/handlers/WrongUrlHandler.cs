using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using FLocal.Core;

namespace FLocal.IISHandler.handlers {
	class WrongUrlHandler : AbstractGetHandler  {

		protected override string templateName {
			get {
				return "WrongUrl.xslt";
			}
		}

		protected override IEnumerable<XElement> getSpecificData(WebContext context) {
			context.LogError(new WrongUrlException());
			return new XElement[] {
				new XElement("path", context.httprequest.Path)
			};
		}

	}
}
