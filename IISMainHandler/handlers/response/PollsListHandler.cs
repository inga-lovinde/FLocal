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

	class PollsListHandler : AbstractGetHandler<FLocal.Common.URL.polls.List> {

		override protected string templateName {
			get {
				return "PollsList.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificData(WebContext context) {
			PageOuter pageOuter = PageOuter.createFromUrl(this.url, context.userSettings.threadsPerPage);
			IEnumerable<Poll> polls = Poll.LoadByIds(
				from stringId in Config.instance.mainConnection.LoadIdsByConditions(
					Poll.TableSpec.instance,
					new EmptyCondition(),
					pageOuter,
					new JoinSpec[0],
					new SortSpec[] {
						new SortSpec(
							Poll.TableSpec.instance.getIdSpec(),
							pageOuter.reversed
						)
					}
				) select int.Parse(stringId)
			);

			return new XElement[] {
				new XElement("polls",
					from poll in polls select poll.exportToXml(context),
					pageOuter.exportToXml(2, 5, 2)
				)
			};
		}

	}

}