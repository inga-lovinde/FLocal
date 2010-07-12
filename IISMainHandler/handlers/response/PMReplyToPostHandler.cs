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

	class PMReplyToPostHandler : AbstractGetHandler {

		override protected string templateName {
			get {
				return "PMReplyToPost.xslt";
			}
		}

		override protected XElement[] getSpecificData(WebContext context) {

			Post post = Post.LoadById(int.Parse(context.requestParts[1]));

			string quoted = context.httprequest.Form["data"];
			if(quoted != null) quoted = quoted.Trim();
			if(quoted == null || quoted == "") {
				if(post.revision.HasValue) {
					quoted = post.latestRevision.body.Trim();
				}
			}

			return new XElement[] {
				post.thread.board.exportToXml(context, false),
				post.thread.exportToXml(context, false),
				post.exportToXmlWithoutThread(context, false),
				new XElement("receiver", Account.LoadByUser(post.poster).exportToXml(context)),
				new XElement("quoted", quoted),
			};
		}
	}

}