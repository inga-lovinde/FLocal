﻿using System;
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
			FLocal.Common.Config.ReInit(ConfigurationManager.AppSettings);

			WebContext context = new WebContext(httpcontext);
			ISpecificHandler handler = HandlersFactory.getHandler(context);
			handler.Handle(context);
		}

	}
}
