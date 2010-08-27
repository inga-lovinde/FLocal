using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core;
using FLocal.Common;
using System.Xml.Linq;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.response {
	class MigrateAccountHandler : AbstractGetHandler {

		protected override string templateName {
			get {
				return "MigrateAccount.xslt";
			}
		}

		protected override IEnumerable<XElement> getSpecificData(WebContext context) {
			string username;
			if(context.httprequest.Form["username"] != null && context.httprequest.Form["username"] != "") {
				username = context.httprequest.Form["username"];
			} else {
				if(context.requestParts.Length != 2) {
					throw new CriticalException("Username is not specified");
				}
				username = context.requestParts[1];
			}
			Account account = Account.LoadByName(username);
			if(!account.needsMigration) throw new FLocalException("Already migrated");
			string key = Util.RandomString(8, Util.RandomSource.LETTERS_DIGITS);
			return new XElement[] {
				new XElement("migrationInfo",
					account.exportToXml(context),
					new XElement("key", key),
					new XElement("check", Util.md5(key + " " + Config.instance.SaltMigration + " " + account.id))
				),
			};
		}

	}
}
