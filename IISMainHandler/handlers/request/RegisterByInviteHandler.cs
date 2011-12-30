using System;
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

namespace FLocal.IISHandler.handlers.request {
	class RegisterByInviteHandler : AbstractNewAccountHandler {

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

			Invite invite = Invite.LoadById(int.Parse(context.httprequest.Form["inviteId"]));
			if(invite.isUsed) throw new FLocalException("Invite is already used");
			if(context.httprequest.Form["password"] != context.httprequest.Form["password2"]) throw new FLocalException("Passwords mismatch");
			return invite.createAccount(context.httprequest.Form["code"], context.httprequest.Form["login"], context.httprequest.Form["password"], context.httprequest.UserHostAddress, context.httprequest.Form["registrationEmail"]);
		}

	}
}
