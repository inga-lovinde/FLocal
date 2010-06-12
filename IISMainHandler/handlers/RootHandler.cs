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

		override protected XElement[] getSpecificData(WebContext context) {
			return new XElement[0];
		}

	}

}