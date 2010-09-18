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

	class BoardHandler : AbstractGetHandler<FLocal.Common.URL.forum.board.Threads> {

		override protected string templateName {
			get {
				return "Board.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificData(WebContext context) {
			Board board = this.url.board;
			PageOuter pageOuter = PageOuter.createFromUrl(this.url, context.userSettings.threadsPerPage);
			IEnumerable<Thread> threads = board.getThreads(pageOuter, pageOuter.descendingDirection);
			XElement[] result = new XElement[] {
				new XElement("currentLocation", board.exportToXmlSimpleWithParent(context)),
				new XElement("boards", from subBoard in board.subBoards select subBoard.exportToXml(context, Board.SubboardsOptions.FirstLevel)),
				new XElement("threads", 
					from thread in threads select thread.exportToXml(context),
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