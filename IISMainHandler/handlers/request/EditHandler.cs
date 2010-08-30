using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.request {
	class EditHandler : AbstractNewMessageHandler {

		protected override string templateName {
			get {
				return "result/MessageEdited.xslt";
			}
		}

		protected override XElement[] Do(WebContext context) {

			Post post = Post.LoadById(int.Parse(context.httprequest.Form["postId"]));
			XElement postXml = post.exportToXml(context, false);
			post.Edit(
				context.session.account.user,
				this.getTitle(context),
				this.getBody(context),
				PostLayer.LoadById(int.Parse(context.httprequest.Form["layerId"]))
			);
			
			return new XElement[] {
				post.thread.board.exportToXml(context, false),
				postXml
			};
		}

	}
}
