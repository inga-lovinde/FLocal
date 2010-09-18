using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using FLocal.Common;
using FLocal.Common.dataobjects;
using FLocal.Core.DB;

namespace FLocal.IISHandler.handlers.response {

	class BoardAsThreadHandler : AbstractGetHandler<FLocal.Common.URL.forum.board.Headlines> {

		override protected string templateName {
			get {
				return "BoardAsThread.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificData(WebContext context) {
			Board board = this.url.board;
			PageOuter pageOuter = PageOuter.createFromUrl(this.url, context.userSettings.postsPerPage);
			IEnumerable<Thread> threads = board.getThreads(
				pageOuter,
				new SortSpec[] {
					new SortSpec(
						Thread.TableSpec.instance.getColumnSpec(Thread.TableSpec.FIELD_ID),
						pageOuter.ascendingDirection
					),
				}
			);
			return new XElement[] {
				new XElement("currentLocation", board.exportToXmlSimpleWithParent(context)),
				new XElement("boards", from subBoard in board.subBoards select subBoard.exportToXml(context, Board.SubboardsOptions.FirstLevel)),
				new XElement("posts", 
					from thread in threads select thread.firstPost.exportToXml(
						context,
						new XElement("specific", thread.exportToXml(context))
					),
					pageOuter.exportToXml(1, 5, 1)
				)
			};
		}

	}

}