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

		override protected IEnumerable<XElement> getSpecificData(WebContext context) {
			PageOuter pageOuter = PageOuter.createFromGet(
				context.requestParts,
				context.userSettings.postsPerPage,
				1
			);
			IEnumerable<Post> posts = Post.LoadByIds(
				from stringId
				in Config.instance.mainConnection.LoadIdsByConditions(
					Post.TableSpec.instance,
					new ComparisonCondition(
						Post.TableSpec.instance.getIdSpec(),
						ComparisonType.GREATEROREQUAL,
						Thread.FORMALREADMIN.ToString()
					),
					pageOuter,
					new JoinSpec[0],
					new SortSpec[] {
						new SortSpec(
							Post.TableSpec.instance.getIdSpec(),
							pageOuter.descendingDirection
						)
					}
				)
				select int.Parse(stringId)
			);

			XElement[] result = new XElement[] {
				new XElement("posts",
					from post in posts select post.exportToXml(context),
					pageOuter.exportToXml(2, 5, 2)
				)
			};

			return result;
		}

	}

}