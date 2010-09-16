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

namespace FLocal.IISHandler.handlers {

	class ThreadHandler : AbstractGetHandler {

		override protected string templateName {
			get {
				return "Thread.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificData(WebContext context) {
			Thread thread = Thread.LoadById(int.Parse(context.requestParts[1]));
			PageOuter pageOuter = PageOuter.createFromGet(
				context.requestParts,
				context.userSettings.postsPerPage,
				new Dictionary<char,Func<long>> {
					{
						'p',
						() => Config.instance.mainConnection.GetCountByConditions(
							Post.TableSpec.instance,
							new ComplexCondition(
								ConditionsJoinType.AND,
								new ComparisonCondition(
									Post.TableSpec.instance.getColumnSpec(Post.TableSpec.FIELD_THREADID),
									ComparisonType.EQUAL,
									thread.id.ToString()
								),
								new ComparisonCondition(
									Post.TableSpec.instance.getIdSpec(),
									ComparisonType.LESSTHAN,
									int.Parse(context.requestParts[2].PHPSubstring(1)).ToString()
								)
							)
						)
					}
				},
				2
			);
			IEnumerable<Post> posts = thread.getPosts(pageOuter, pageOuter.ascendingDirection);

			int lastReadId = 0;
			if(context.session != null) {
				lastReadId = thread.getLastReadId(context.session.account);
			}

			XElement[] result = new XElement[] {
				new XElement("currentLocation", thread.exportToXmlSimpleWithParent(context)),
				thread.exportToXml(context),
				new XElement("posts",
					from post in posts select post.exportToXml(
						context,
						new XElement("isUnread", (post.id > lastReadId).ToPlainString())
					),
					pageOuter.exportToXml(2, 5, 2)
				)
			};

			thread.incrementViewsCounter();
			if((context.session != null) && (posts.Count() > 0)) {
				thread.markAsRead(context.session.account, posts.Min(), posts.Max());
			}

			return result;
		}

	}

}