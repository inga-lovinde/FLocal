using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Common.dataobjects;
using FLocal.Importer;
using System.Text.RegularExpressions;
using FLocal.Core;
using FLocal.Common;
using FLocal.Common.actions;
using System.Web;

namespace FLocal.IISHandler.handlers.request {
	class LogoutHandler : AbstractPostHandler {

		protected override string templateName {
			get {
				return "result/Logout.xslt";
			}
		}

		protected override XElement[] Do(WebContext context) {
			if(context.session.id.ToString().ToLower() != context.httprequest.QueryString["sessionKey"].ToLower()) {
				throw new FLocalException("Wrong session id");
			}
			context.session.delete();

			HttpCookie sessionCookie = context.createCookie(Config.instance.CookiesPrefix + "_session");
			sessionCookie.Value = "";
			sessionCookie.Expires = DateTime.Now.AddDays(-1);
			context.httpresponse.AppendCookie(sessionCookie);
	
			context.session = null;
			
			return new XElement[0];
		}

	}
}
