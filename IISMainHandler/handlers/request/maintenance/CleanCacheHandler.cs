using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Common;

namespace FLocal.IISHandler.handlers.request.maintenance {
	class CleanCacheHandler : AbstractNewMessageHandler {

		protected override string templateName {
			get {
				return "result/maintenance/CacheCleaned.xslt";
			}
		}

		protected override XElement[] Do(WebContext context) {
			if(context.account.user.name != Config.instance.AdminUserName) {
				throw new FLocalException("access denied");
			}
			string table = context.httprequest.Form["table"].Trim();
			int start = int.Parse(context.httprequest.Form["start"]);
			int length = int.Parse(context.httprequest.Form["length"]);
			ISqlObjectTableSpec tableSpec = TableManager.TABLES[table];
			for(int i=0; i<length; i++) {
				tableSpec.refreshSqlObject(start+i);
			}
			return new XElement[] {
			};
		}

	}
}
