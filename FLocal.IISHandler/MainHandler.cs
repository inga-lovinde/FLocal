﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using Web.Core;
using FLocal.Common;
using FLocal.Patcher.Common;

namespace FLocal.IISHandler {
	public class MainHandler : IHttpHandler {

		private static readonly Counter counter = new Counter();

		public bool IsReusable {
			get { return true; }
		}

		private void doProcessRequest(HttpContext httpcontext) {

			if(PatcherInfo.instance.IsNeedsPatching) {
				throw new FLocalException("DB is outdated");
			}

			Uri current = httpcontext.Request.Url;
			if(!current.Host.EndsWith(Config.instance.BaseHost)) {
				throw new FLocalException("Wrong host: " + current.Host + " (expected *" + Config.instance.BaseHost + ")");
			}
			if(Config.instance.forceHttps && !httpcontext.Request.IsSecureConnection) {
				throw new FLocalException("Only HTTPS connections are allowed");
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
			Initializer.instance.Initialize();

			DateTime start = DateTime.Now;
			int requestNumber = counter.GetCurrentValueAndIncrement();
			try {
				Config.instance.Logger.Log("Began serving request #" + requestNumber + ": " + context.Request.Url.AbsoluteUri);
				this.doProcessRequest(context);
				Config.instance.Logger.Log("Done serving request #" + requestNumber + "; " + (DateTime.Now-start).TotalSeconds + " seconds spent");
			} catch(RedirectException e) {
				Config.instance.Logger.Log("Done serving request #" + requestNumber + "; " + (DateTime.Now-start).TotalSeconds + " seconds spent (redirected)");
				context.Response.Redirect(e.newUrl);
			}
		}

	}
}
