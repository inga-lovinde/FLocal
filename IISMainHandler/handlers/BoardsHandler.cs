using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using FLocal.Common;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers {

	class BoardsHandler : AbstractGetHandler {

		override protected string templateName {
			get {
				return "Boards.xslt";
			}
		}

		override protected XElement[] getSpecificData(WebContext context) {
			return new XElement[] { new XElement("categories", from category in Category.allCategories select category.exportToXmlForMainPage(context)) };
		}

	}

}