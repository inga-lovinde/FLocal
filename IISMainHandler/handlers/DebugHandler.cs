using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FLocal.IISHandler.handlers {
	class DebugHandler : ISpecificHandler {

		private string type;

		public DebugHandler(string type) {
			this.type = type;
		}

		public void Handle(WebContext context) {
			context.httpresponse.ContentType = "text/plain";
			context.httpresponse.WriteLine("Page: " + this.type);
			context.httpresponse.WriteLine("Path: " + context.httprequest.Path);
			context.httpresponse.WriteLine("PathInfo: " + context.httprequest.PathInfo);
			context.httpresponse.WriteLine("AppInfo: " + context.config.AppInfo);
		}

	}
}
