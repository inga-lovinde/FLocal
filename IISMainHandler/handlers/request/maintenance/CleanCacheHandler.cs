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
			foreach(int id in Enumerable.Range(start, length)) {
				try {
					if(tableSpec is IComplexSqlObjectTableSpec) {
						((IComplexSqlObjectTableSpec)tableSpec).refreshSqlObjectAndRelated(id);
					} else {
						tableSpec.refreshSqlObject(id);
					}
				} catch(NotFoundInDBException) {
				}
			}
			return new XElement[] {
			};
		}

	}
}
