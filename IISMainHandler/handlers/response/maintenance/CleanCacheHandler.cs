using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Web.Core;
using FLocal.Common;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.response.maintenance {

	class CleanCacheHandler : AbstractGetHandler<FLocal.Common.URL.maintenance.CleanCache> {

		override protected string templateName {
			get {
				return "maintenance/CleanCache.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificData(WebContext context) {
			return new XElement[] {
				new XElement(
					"tables",
					from kvp in TableManager.TABLES select new XElement("table", kvp.Key)
				)
			};
		}
	}

}