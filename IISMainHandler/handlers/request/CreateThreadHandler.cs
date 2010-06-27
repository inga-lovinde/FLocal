using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.request {
	class CreateThreadHandler : AbstractNewMessageHandler {

		protected override XElement[] Do(WebContext context) {

			Thread newThread = Board.LoadById(
				int.Parse(context.httprequest.Form["board"])
			).CreateThread(
				context.session.account.user,
				this.getTitle(context),
				this.getBody(context),
				int.Parse(context.httprequest.Form["layerId"])
			);
			
			Post newPost = newThread.firstPost;

			newThread.markAsRead(context.session.account, newPost, newPost);

			return new XElement[] { newPost.exportToXmlWithoutThread(context, false) };
		}
	}
}
