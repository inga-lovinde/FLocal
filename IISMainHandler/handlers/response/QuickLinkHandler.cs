using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Core;
using FLocal.Common;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.response {
	class QuickLinkHandler : RedirectGetHandler<FLocal.Common.URL.QuickLink> {

		protected override string getRedirectUrl(WebContext context) {
			return this.url.link.url;
		}

	}
}
