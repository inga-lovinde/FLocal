﻿using System;
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

	class UserPollsParticipatedHandler : AbstractUserGetHandler<FLocal.Common.URL.users.user.PollsParticipated> {

		override protected string templateName {
			get {
				return "UserPollsParticipated.xslt";
			}
		}

		override protected IEnumerable<XElement> getUserSpecificData(WebContext context, User user) {
			PageOuter pageOuter = PageOuter.createFromUrl(this.url, context.userSettings.threadsPerPage);
			IEnumerable<Poll.Vote> votes = Poll.Vote.LoadByIds(
				from stringId in Config.instance.mainConnection.LoadIdsByConditions(
					Poll.Vote.TableSpec.instance,
					new ComparisonCondition(
						Poll.Vote.TableSpec.instance.getColumnSpec(Poll.Vote.TableSpec.FIELD_USERID),
						ComparisonType.EQUAL,
						user.id.ToString()
					),
					pageOuter,
					new JoinSpec[0],
					new SortSpec[] {
						new SortSpec(
							Poll.Vote.TableSpec.instance.getIdSpec(),
							pageOuter.descendingDirection
						)
					}
				) select int.Parse(stringId)
			);

			return new XElement[] {
				new XElement("polls",
					from vote in votes select vote.poll.exportToXml(context),
					pageOuter.exportToXml(2, 5, 2)
				)
			};
		}

	}

}