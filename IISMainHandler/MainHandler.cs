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

		private void doProcessRequest(HttpContext httpcontext) {

			Uri referer = httpcontext.Request.UrlReferrer;
			if(referer != null && referer.PathAndQuery.StartsWith("/static") && !httpcontext.Request.Path.StartsWith("/static") {
				throw new HttpException(403, "You have come from the static page '" + referer + "'");
			}

			if(!FLocal.Common.Config.isInitialized) {
				lock(typeof(FLocal.Common.Config)) {
					if(!FLocal.Common.Config.isInitialized) {
						FLocal.Common.Config.Init(ConfigurationManager.AppSettings);
					}
				}
			}

			WebContext context = new WebContext(httpcontext);
			ISpecificHandler handler = HandlersFactory.getHandler(context);
			handler.Handle(context);
		}

		public void ProcessRequest(HttpContext context) {
			try {
				this.doProcessRequest(context);
			} catch(RedirectException e) {
				context.Response.Redirect(e.newUrl);
			}
		}

	}
}
