using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;

namespace FLocal.IISHandler {
	public class MainHandler : IHttpHandler {

		public bool IsReusable {
			get { return true; }
		}

		public void ProcessRequest(HttpContext httpcontext) {

			Uri referer = httpcontext.Request.UrlReferrer;
			if(referer != null && referer.PathAndQuery.StartsWith("/static")) {
				throw new HttpException(403, "You have come from the static page");
			}

			if(!FLocal.Common.Config.isInitialized) FLocal.Common.Config.Init(ConfigurationManager.AppSettings);

			WebContext context = new WebContext(httpcontext);
			ISpecificHandler handler = HandlersFactory.getHandler(context);
			handler.Handle(context);
		}

	}
}
