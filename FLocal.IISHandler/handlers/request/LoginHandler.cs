﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Common.dataobjects;
using FLocal.Migration.Gateway;
using System.Text.RegularExpressions;
using Web.Core;
using FLocal.Common;
using FLocal.Common.actions;
using System.Web;

namespace FLocal.IISHandler.handlers.request {
	class LoginHandler : AbstractPostHandler {

		protected override string templateName {
			get {
				return "result/Login.xslt";
			}
		}

		protected override bool shouldBeLoggedIn {
			get { return false; }
		}

		protected override bool shouldBeGuest {
			get { return true; }
		}

		protected override XElement[] Do(WebContext context) {
			
			if(Config.instance.IsMigrationEnabled) {
				try {
					Account tmpAccount = Account.LoadByName(context.httprequest.Form["name"]);
					if(tmpAccount.needsMigration) {
						throw new RedirectException("/My/Login/MigrateAccount/" + context.httprequest.Form["name"]);
					}
				} catch(NotFoundInDBException) {
				}
			}

			Account account = Account.tryAuthorize(context.httprequest.Form["name"], context.httprequest.Form["password"]);
			Session session = Session.create(account);

			HttpCookie sessionCookie = context.createCookie(Config.instance.CookiesPrefix + "_session");
			sessionCookie.Value = session.sessionKey;
			sessionCookie.Expires = DateTime.Now.AddSeconds(Config.instance.SessionLifetime);
			context.httpresponse.AppendCookie(sessionCookie);
			context.session = session;

			return new XElement[0];
		}

	}
}
