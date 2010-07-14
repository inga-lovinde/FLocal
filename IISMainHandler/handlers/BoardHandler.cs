using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using FLocal.Common;
using FLocal.Common.dataobjects;
using FLocal.Core;
using FLocal.Core.DB;

namespace FLocal.IISHandler.handlers {

	class BoardHandler : AbstractGetHandler {

		override protected string templateName {
			get {
				return "Board.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificData(WebContext context) {
			Board board = Board.LoadById(int.Parse(context.requestParts[1]));
			PageOuter pageOuter = PageOuter.createFromGet(context.requestParts, context.userSettings.threadsPerPage, 2);
			IEnumerable<Thread> threads = board.getThreads(pageOuter);
			XElement[] result = new XElement[] {
				new XElement("currentLocation", board.exportToXmlSimpleWithParent(context)),
				new XElement("boards", from subBoard in board.subBoards select subBoard.exportToXml(context, true)),
				new XElement("threads", 
					from thread in threads select thread.exportToXml(context, false),
					pageOuter.exportToXml(1, 5, 1)
				)
			};

			if(context.session != null) {
				if(pageOuter.start == 0) {
					board.markAsRead(context.session.account);
				}
			}

			return result;
		}

	}

}