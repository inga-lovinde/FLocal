using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.request {
	class ReplyHandler : AbstractNewMessageHandler {

		protected override XElement[] Do(WebContext context) {
//			Post post = Post.LoadById(int.Parse(context.httprequest.Form["parent"]));
			//int desiredLayerId = Math.Min(context.session.account.user.getMinAllowedLayer(post.thread.board), int.Parse(context.httprequest.Form["layer"]));

			Post newPost = Post.LoadById(
				int.Parse(context.httprequest.Form["parent"])
			).Reply(
				context.session.account.user,
				this.getTitle(context),
				this.getBody(context),
				PostLayer.LoadById(int.Parse(context.httprequest.Form["layerId"]))
			);
			
			newPost.thread.markAsRead(context.session.account, newPost, newPost);

			return new XElement[] {
				newPost.thread.board.exportToXml(context, false),
				newPost.exportToXml(context)
			};
		}

	}
}
