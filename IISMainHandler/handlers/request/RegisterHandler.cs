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

namespace FLocal.IISHandler.handlers.request {
	class RegisterHandler : AbstractNewAccountHandler {

		protected override Account DoCreateAccount(WebContext context) {
			if(Config.instance.IsMigrationEnabled) {
				try {
					Account tmpAccount = Account.LoadByName(context.httprequest.Form["login"]);
					if(tmpAccount.needsMigration) {
						throw new RedirectException("/My/Login/MigrateAccount/" + context.httprequest.Form["login"]);
					}
				} catch(NotFoundInDBException) {
				}
			}

			if(!LocalNetwork.IsLocalNetwork(context.remoteHost)) throw new FLocalException("IP '" + context.remoteHost.ToString() + "' is not allowed");
			if(context.httprequest.Form["password"] != context.httprequest.Form["password2"]) throw new FLocalException("Passwords mismatch");
			return Account.createAccount(context.httprequest.Form["login"], context.httprequest.Form["password"], context.httprequest.UserHostAddress, context.httprequest.Form["registrationEmail"]);
		}

	}
}
