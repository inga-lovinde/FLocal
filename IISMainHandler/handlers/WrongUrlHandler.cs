﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace FLocal.IISHandler.handlers {
	class WrongUrlHandler : AbstractGetHandler  {

		protected override string templateName {
			get {
				return "WrongUrl.xslt";
			}
		}

		protected override XElement[] getSpecificData(WebContext context) {
			return new XElement[] {
				new XElement("path", context.httprequest.Path)
			};
		}

	}
}
