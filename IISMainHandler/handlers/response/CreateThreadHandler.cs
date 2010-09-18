using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Common;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.response {

	class CreateThreadHandler : AbstractNewMessageHandler<FLocal.Common.URL.forum.board.NewThread> {

		override protected string templateName {
			get {
				return "NewThread.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificNewMessageData(WebContext context) {
			Board board = this.url.board;

			return new XElement[] {
				board.exportToXml(context, Board.SubboardsOptions.None),
				board.exportLayersInfoForUser(context),
			};
		}
	}

}