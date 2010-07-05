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
	class MigrateAccountHandler : AbstractPostHandler {

		protected override string templateName {
			get {
				return "result/MigrateAccount.xslt";
			}
		}

		protected override bool shouldBeLoggedIn {
			get { return false; }
		}

		protected override bool shouldBeGuest {
			get { return true; }
		}

		protected override XElement[] Do(WebContext context) {

			if(context.httprequest.Form["constitution"] != "constitution") {
				throw new FLocalException("constitution not accepted");
			}
			if(context.httprequest.Form["showPostsToAll"] != "showPostsToAll") {
				throw new FLocalException("publicity not accepted");
			}
			if(context.httprequest.Form["law"] != "law") {
				throw new FLocalException("laws not accepted");
			}

			Account account = Account.LoadById(int.Parse(context.httprequest.Form["accountId"]));
			if(!account.needsMigration) throw new FLocalException("Already migrated");
			string userInfo = ShallerGateway.getUserInfoAsString(account.user.name);
			Regex regex = new Regex("\\(fhn\\:([a-z0-9]+)\\)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
			Match match = regex.Match(userInfo);
			if(!match.Success) throw new FLocalException("key (fhn:***) not found on user info page");
			string check = Util.md5(match.Groups[1].Value +  " " + Config.instance.SaltMigration + " " + account.id);
			if(check != context.httprequest["check"]) throw new FLocalException("Wrong key (fhn:" + match.Groups[1].Value + ")");
			if(context.httprequest.Form["password"] != context.httprequest.Form["password2"]) throw new FLocalException("Passwords mismatch");
			account.migrate(context.httprequest.Form["password2"]);
			return new XElement[0];
		}

	}
}
