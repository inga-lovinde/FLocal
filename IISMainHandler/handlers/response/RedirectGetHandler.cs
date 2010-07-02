﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FLocal.IISHandler.handlers.response {
	abstract class RedirectGetHandler : AbstractGetHandler {

		protected override string templateName {
			get {
				return null;
			}
		}

		abstract protected string getRedirectUrl(WebContext context);

		protected override XElement[] getSpecificData(WebContext context) {
			throw new RedirectException(this.getRedirectUrl(context));
		}

	}
}
