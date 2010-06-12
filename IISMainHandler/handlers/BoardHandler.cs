using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using FLocal.Common;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers {

	class BoardHandler : AbstractGetHandler {

		override protected string templateName {
			get {
				return "Board.xslt";
			}
		}

		override protected XElement[] getSpecificData(WebContext context) {
			Board board = Board.LoadById(int.Parse(context.requestParts[1]));
			return new XElement[] {
				new XElement("currentBoard", board.exportToXmlSimpleWithParent(context)),
				new XElement("boards", from subBoard in board.subBoards select subBoard.exportToXml(context, true))
			};
		}

	}

}