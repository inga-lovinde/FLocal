using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using FLocal.Common;
using FLocal.Common.dataobjects;
using Web.Core;
using Web.Core.DB;
using Web.Core.DB.conditions;

namespace FLocal.IISHandler.handlers.response {

	class UserRepliesHandler : AbstractUserGetHandler<FLocal.Common.URL.users.user.Mentions> {

		override protected string templateName {
			get {
				return "UserReplies.xslt";
			}
		}

		override protected IEnumerable<XElement> getUserSpecificData(WebContext context, User user) {
			PageOuter pageOuter = PageOuter.createFromUrl(this.url, context.userSettings.postsPerPage);
			IEnumerable<Post> posts = user.getMentions(pageOuter, pageOuter.descendingDirection);

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