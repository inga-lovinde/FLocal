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

	class ReplyHandler : AbstractGetHandler {

		override protected string templateName {
			get {
				return "PostReply.xslt";
			}
		}

		override protected XElement[] getSpecificData(WebContext context) {
			Post post = Post.LoadById(int.Parse(context.requestParts[1]));

			return new XElement[] {
				post.thread.board.exportToXml(context, false),
				post.thread.exportToXml(context, false),
				post.exportToXmlWithoutThread(context, false),
				new XElement("layers",
					from layer in PostLayer.allLayers select layer.exportToXml(context)
				),
			};
		}
	}

}