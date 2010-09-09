using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using FLocal.Common;
using FLocal.Common.dataobjects;
using FLocal.Core;
using FLocal.Core.DB;
using FLocal.Core.DB.conditions;

namespace FLocal.IISHandler.handlers.response {

	class UserRepliesHandler : AbstractUserGetHandler {

		override protected string templateName {
			get {
				return "UserReplies.xslt";
			}
		}

		override protected IEnumerable<XElement> getUserSpecificData(WebContext context, User user) {
			PageOuter pageOuter = PageOuter.createFromGet(
				context.requestParts,
				context.userSettings.postsPerPage,
				4
			);
			IEnumerable<Post> posts = user.getReplies(pageOuter);

			return new XElement[] {
				user.exportToXmlForViewing(context),
				new XElement("posts",
					from post in posts select post.exportToXml(context),
					pageOuter.exportToXml(2, 5, 2)
				)
			};
		}

	}

}