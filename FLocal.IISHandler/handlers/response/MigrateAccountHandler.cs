﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Core;
using FLocal.Common;
using System.Xml.Linq;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.response {
	class MigrateAccountHandler : AbstractGetHandler<FLocal.Common.URL.my.login.Migrate> {

		protected override string templateName {
			get {
				return "MigrateAccount.xslt";
			}
		}

		protected override IEnumerable<XElement> getSpecificData(WebContext context) {
			if(!Config.instance.IsMigrationEnabled) throw new FLocalException("Migration is disabled");
			string username;
			if(context.httprequest.Form["username"] != null && context.httprequest.Form["username"] != "") {
				username = context.httprequest.Form["username"];
			} else {
				if(string.IsNullOrEmpty(this.url.remainder)) {
					throw new CriticalException("Username is not specified");
				}
				username = this.url.remainder;
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
