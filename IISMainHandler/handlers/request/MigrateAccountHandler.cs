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
	class MigrateAccountHandler : AbstractNewAccountHandler {

		protected override Account DoCreateAccount(WebContext context) {
			Account account = Account.LoadById(int.Parse(context.httprequest.Form["accountId"]));
			if(!account.needsMigration) throw new FLocalException("Account '" + account.name + "' is already migrated");
			string userInfo = ShallerGateway.getUserInfoAsString(account.user.name);
			Regex regex = new Regex("\\(fhn\\:([a-z0-9]+)\\)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
			Match match = regex.Match(userInfo);
			if(!match.Success) throw new FLocalException("key (fhn:***) not found on user info page ( http://forumlocal.ru/showprofile.php?User=" + account.user.name + "&What=login&showlite=l )");
			string check = Util.md5(match.Groups[1].Value +  " " + Config.instance.SaltMigration + " " + account.id);
			if(check != context.httprequest["check"]) throw new FLocalException("Wrong key (fhn:" + match.Groups[1].Value + ")");
			if(context.httprequest.Form["password"] != context.httprequest.Form["password2"]) throw new FLocalException("Passwords mismatch");
			account.migrate(context.httprequest.Form["password"], context.httprequest.UserHostAddress, context.httprequest.Form["registrationEmail"]);
			return account;
		}

	}
}
