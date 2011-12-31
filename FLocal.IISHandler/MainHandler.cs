﻿using System;
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

			Uri current = httpcontext.Request.Url;
			if(!current.Host.EndsWith(Config.instance.BaseHost)) {
				throw new Web.Core.FLocalException("Wrong host: " + current.Host + " (expected *" + Config.instance.BaseHost + ")");
			}
			if(Config.instance.forceHttps && !httpcontext.Request.IsSecureConnection) {
				throw new Web.Core.FLocalException("Only HTTPS connections are allowed");
			}

			Uri referer = httpcontext.Request.UrlReferrer;
			if(referer != null && referer.PathAndQuery.StartsWith("/static") && !httpcontext.Request.Path.StartsWith("/static")) {
				throw new HttpException(403, "You have come from the static page '" + referer + "'");
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
