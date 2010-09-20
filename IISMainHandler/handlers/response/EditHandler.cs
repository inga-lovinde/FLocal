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

	class EditHandler : AbstractNewMessageHandler<FLocal.Common.URL.forum.board.thread.post.Edit> {

		override protected string templateName {
			get {
				return "PostEdit.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificNewMessageData(WebContext context) {
			Post post = this.url.post;

			return new XElement[] {
				new XElement("currentLocation", post.exportToXmlSimpleWithParent(context)),
				post.thread.board.exportToXml(context, Board.SubboardsOptions.None),
				post.thread.exportToXml(context),
				post.exportToXml(context),
				post.latestRevision.exportToXml(context),
				post.thread.board.exportLayersInfoForUser(context),
			};
		}
	}

}