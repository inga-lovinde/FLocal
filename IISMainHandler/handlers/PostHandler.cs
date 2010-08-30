using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Common;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers {

	class PostHandler : AbstractGetHandler {

		override protected string templateName {
			get {
				return "Post.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificData(WebContext context) {
			Post post = Post.LoadById(int.Parse(context.requestParts[1]));

			int lastReadId = 0;
			if(context.session != null) {
				lastReadId = post.thread.getLastReadId(context.session.account);
			}

			XElement[] result = new XElement[] {
				new XElement("currentLocation", post.exportToXmlSimpleWithParent(context)),
				post.thread.exportToXml(context),
				new XElement("posts", post.exportToXml(context, true, new XElement("isUnread", (post.id > lastReadId).ToPlainString())))
			};

			post.thread.incrementViewsCounter();
			if(context.session != null) {
				post.thread.markAsRead(context.session.account, post, post);
			}

			return result;
	}

	}

}