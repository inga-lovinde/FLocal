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
			Board board3 = Board.LoadById(3);
			return new XDocument(
				new XElement("root",
					new XElement("title", Config.instance.AppInfo),
					new XElement("categories",
						new XElement("category",
							new XElement("name", board1.category.name),
							new XElement("boards",
								board1.exportToXml(),
								board2.exportToXml()
							)
						),
						new XElement("category",
							new XElement("name", board3.category.name),
							new XElement("boards",
								board3.exportToXml()
							)
						)
					)
				)
			);
		}

	}

}