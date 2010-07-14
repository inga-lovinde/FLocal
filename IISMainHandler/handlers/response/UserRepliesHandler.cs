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

	class UserRepliesHandler : AbstractGetHandler {

		override protected string templateName {
			get {
				return "UserReplies.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificData(WebContext context) {
			User user = User.LoadById(int.Parse(context.requestParts[1]));
			PageOuter pageOuter = PageOuter.createFromGet(
				context.requestParts,
				context.userSettings.postsPerPage,
				3
			);
			IEnumerable<Post> posts = user.getReplies(pageOuter);

			XElement[] result = new XElement[] {
				user.exportToXmlForViewing(context),
				new XElement("posts",
					from post in posts select post.exportToXmlWithoutThread(context, true),
					pageOuter.exportToXml(2, 5, 2)
				)
			};

			return result;
		}

	}

}