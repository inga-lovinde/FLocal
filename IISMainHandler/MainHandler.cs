using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using FLocal.Common;

namespace FLocal.IISHandler {
	public class MainHandler : IHttpHandler {

		public bool IsReusable {
			get { return true; }
		}

		private void doProcessRequest(HttpContext httpcontext) {

			Uri referer = httpcontext.Request.UrlReferrer;
			if(referer != null && referer.PathAndQuery.StartsWith("/static") && !httpcontext.Request.Path.StartsWith("/static")) {
				throw new HttpException(403, "You have come from the static page '" + referer + "'");
			}

			if(!Config.isInitialized) {
				lock(typeof(Config)) {
					if(!Config.isInitialized) {
						Config.Init(ConfigurationManager.AppSettings);
					}
				}
			}

			Uri current = httpcontext.Request.Url;
			if(!current.Host.EndsWith(Config.instance.BaseHost)) {
				throw new FLocal.Core.FLocalException("Wrong host: " + current.Host + " (expected *" + Config.instance.BaseHost + ")");
			}

			WebContext context = new WebContext(httpcontext);
			try {
				ISpecificHandler handler = HandlersFactory.getHandler(context);
				handler.Handle(context);
			} catch(WrongUrlException) {
				(new handlers.WrongUrlHandler()).Handle(context);
			}
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
