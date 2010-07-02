using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FLocal.IISHandler.handlers.request {
	abstract class ReturnPostHandler : AbstractPostHandler {

		protected override string templateName {
			get {
				return null;
			}
		}

		abstract protected void _Do(WebContext context);

		protected override XElement[] Do(WebContext context) {
			this._Do(context);
			throw new RedirectException(context.httprequest.UrlReferrer.ToString());
		}

	}
}
