using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace FLocal.IISHandler.handlers {

	class RootHandler : AbstractGetHandler {

		override protected string templateName {
			get {
				return "Root.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificData(WebContext context) {
			Uri url = context.httprequest.Url;
			return new XElement[] {
				new XElement(
					"url",
					new XElement("host", url.Host),
					new XElement("port", url.Port)
				),
			};
		}

	}

}