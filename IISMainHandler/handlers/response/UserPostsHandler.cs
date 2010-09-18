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

	class UserPostsHandler : AbstractUserGetHandler<FLocal.Common.URL.users.user.Posts> {

		override protected string templateName {
			get {
				return "UserPosts.xslt";
			}
		}

		override protected IEnumerable<XElement> getUserSpecificData(WebContext context, User user) {
			PageOuter pageOuter = PageOuter.createFromUrl(this.url, context.userSettings.postsPerPage);
			IEnumerable<Post> posts = user.getPosts(pageOuter, pageOuter.descendingDirection);

			return new XElement[] {
				new XElement("posts",
					from post in posts select post.exportToXml(context),
					pageOuter.exportToXml(2, 5, 2)
				)
			};
		}

	}

}