using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using FLocal.Common;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers {

	class PostHandler : AbstractGetHandler {

		override protected string templateName {
			get {
				return "Post.xslt";
			}
		}

		override protected XElement[] getSpecificData(WebContext context) {
			Post post = Post.LoadById(int.Parse(context.requestParts[1]));
			post.thread.incrementViewsCounter();
			return new XElement[] {
				new XElement("currentLocation", post.exportToXmlSimpleWithParent(context)),
				new XElement("posts", post.exportToXmlWithoutThread(context, true))
			};
		}

	}

}