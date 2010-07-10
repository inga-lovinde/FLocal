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

	class AllPostsHandler : AbstractGetHandler {

		override protected string templateName {
			get {
				return "AllPosts.xslt";
			}
		}

		override protected XElement[] getSpecificData(WebContext context) {
			PageOuter pageOuter = PageOuter.createFromGet(
				context.requestParts,
				context.userSettings.postsPerPage,
				1
			);
			IEnumerable<Post> posts = from stringId in Config.instance.mainConnection.LoadIdsByConditions(Post.TableSpec.instance, new EmptyCondition(), pageOuter, new JoinSpec[0], new SortSpec[] { new SortSpec(Post.TableSpec.instance.getIdSpec(), false) }) select Post.LoadById(int.Parse(stringId));

			XElement[] result = new XElement[] {
				new XElement("posts",
					from post in posts select post.exportToXmlWithoutThread(context, true),
					pageOuter.exportToXml(2, 5, 2)
				)
			};

			return result;
		}

	}

}