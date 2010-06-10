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

		override protected XDocument getData(WebContext context) {
			Board board1 = Board.LoadById(1);
			Board board2 = Board.LoadById(2);
			Board board3 = Board.LoadById(4);
			return new XDocument(
				new XElement("root",
					new XElement("title", Config.instance.AppInfo),
					new XElement("categories", from category in Category.allCategories select category.exportToXmlForMainPage())
				)
			);
		}

	}

}