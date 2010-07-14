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

	class CreateThreadHandler : AbstractNewMessageHandler {

		override protected string templateName {
			get {
				return "NewThread.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificNewMessageData(WebContext context) {
			Board board = Board.LoadById(int.Parse(context.requestParts[1]));

			return new XElement[] {
				board.exportToXml(context, false),
				new XElement("layers",
					from layer in PostLayer.allLayers select layer.exportToXml(context)
				),
			};
		}
	}

}