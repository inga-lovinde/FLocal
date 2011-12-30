using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Web.Core;
using FLocal.Common;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.response {

	class ReplyHandler : AbstractNewMessageHandler<FLocal.Common.URL.forum.board.thread.post.Reply> {

		override protected string templateName {
			get {
				return "PostReply.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificNewMessageData(WebContext context) {
			
			Post post = this.url.post;

			string quoted = context.httprequest.Form["data"];
			if(quoted != null) quoted = quoted.Trim();
			if(quoted == null || quoted == "") {
				if(post.revision.HasValue) {
					quoted = UBBParser.StripQuotes(post.latestRevision.body).Trim();
				}
			}

			return new XElement[] {
				new XElement("currentLocation", post.exportToXmlSimpleWithParent(context)),
				post.thread.board.exportToXml(context, Board.SubboardsOptions.None),
				post.thread.exportToXml(context),
				post.exportToXml(context),
				post.thread.board.exportLayersInfoForUser(context),
				new XElement("quoted", quoted),
			};
		}
	}

}