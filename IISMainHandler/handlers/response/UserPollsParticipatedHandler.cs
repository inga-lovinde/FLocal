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

	class UserPollsParticipatedHandler : AbstractGetHandler {

		override protected string templateName {
			get {
				return "UserPollsParticipated.xslt";
			}
		}

		override protected XElement[] getSpecificData(WebContext context) {
			User user = User.LoadById(int.Parse(context.requestParts[1]));
			PageOuter pageOuter = PageOuter.createFromGet(
				context.requestParts,
				context.userSettings.postsPerPage,
				3
			);
			IEnumerable<Poll.Vote> votes = Poll.Vote.LoadByIds(
				from stringId in Config.instance.mainConnection.LoadIdsByConditions(
					Poll.Vote.TableSpec.instance,
					new ComparisonCondition(
						Poll.Vote.TableSpec.instance.getColumnSpec(Poll.Vote.TableSpec.FIELD_USERID),
						ComparisonType.EQUAL,
						context.session.account.user.id.ToString()
					),
					pageOuter,
					new JoinSpec[0],
					new SortSpec[] {
						new SortSpec(
							Poll.Vote.TableSpec.instance.getIdSpec(),
							false
						)
					}
				) select int.Parse(stringId)
			);

			XElement[] result = new XElement[] {
				user.exportToXmlForViewing(context),
				new XElement("polls",
					from vote in votes select vote.poll.exportToXml(context),
					pageOuter.exportToXml(2, 5, 2)
				)
			};

			return result;
		}

	}

}