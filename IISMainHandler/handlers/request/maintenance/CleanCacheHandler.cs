using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Common;

namespace FLocal.IISHandler.handlers.request.maintenance {
	class CleanCacheHandler : AbstractPostHandler {

		protected override string templateName {
			get {
				return "result/maintenance/CacheCleaned.xslt";
			}
		}

		protected override XElement[] Do(WebContext context) {
			if(context.account.user.name != Config.instance.AdminUserName) {
				throw new FLocalException("access denied");
			}
			int start = int.Parse(context.httprequest.Form["start"]);
			int length = int.Parse(context.httprequest.Form["length"]);
			ISqlObjectTableSpec tableSpec = TableManager.TABLES[context.httprequest.Form["table"].Trim()];
			Action<int> refresher = (tableSpec is IComplexSqlObjectTableSpec)
				? ((IComplexSqlObjectTableSpec)tableSpec).refreshSqlObjectAndRelated
				: (Action<int>)tableSpec.refreshSqlObject;
			foreach(int id in Enumerable.Range(start, length)) {
				try {
					refresher(id);
				} catch(NotFoundInDBException) {
				}
			}
			return new XElement[] {
			};
		}

	}
}
