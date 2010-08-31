using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.request {
	class PunishHandler : AbstractPostHandler {

		protected override string templateName {
			get {
				return "result/MessagePunished.xslt";
			}
		}

		protected override XElement[] Do(WebContext context) {

			Post post = Post.LoadById(int.Parse(context.httprequest.Form["postId"]));
			XElement postXml = post.exportToXml(context);
			post.Punish(
				context.session.account,
				PunishmentType.LoadById(int.Parse(context.httprequest.Form["punishmentTypeId"])),
				context.httprequest.Form["comment"]
			);
			
			return new XElement[] {
				post.thread.board.exportToXml(context, false),
				postXml
			};
		}

	}
}
