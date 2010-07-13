using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using FLocal.Core;

namespace FLocal.IISHandler.handlers.response {
	class RobotsHandler : ISpecificHandler {

		public RobotsHandler() {
		}

		public void Handle(WebContext context) {
			context.httpresponse.ContentType = "text/plain";
			context.httpresponse.WriteLine("User-agent: *");
			context.httpresponse.WriteLine("Disallow: /");
		}

	}
}
