using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core;
using FLocal.Common;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.response {
	class QuickLinkHandler : RedirectGetHandler {

		protected override string getRedirectUrl(WebContext context) {
			return QuickLink.LoadByName(context.requestParts[1]).url;
		}

	}
}
