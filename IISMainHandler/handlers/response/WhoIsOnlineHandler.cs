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

	class WhoIsOnlineHandler : AbstractGetHandler {

		override protected string templateName {
			get {
				return "WhoIsOnline.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificData(WebContext context) {
			PageOuter pageOuter = PageOuter.createUnlimited(context.userSettings.usersPerPage);
			IEnumerable<Session> sessions = 
				from stringId in Config.instance.mainConnection.LoadIdsByConditions(
					Session.TableSpec.instance,
					new ComplexCondition(
						ConditionsJoinType.AND,
						new ComparisonCondition(
							Session.TableSpec.instance.getColumnSpec(Session.TableSpec.FIELD_LASTHUMANACTIVITY),
							Core.DB.conditions.ComparisonType.GREATEROREQUAL,
							DateTime.Now.Subtract(Config.instance.ActivityThreshold).ToUTCString()
						),
						new	ComparisonCondition(
							Session.TableSpec.instance.getColumnSpec(Session.TableSpec.FIELD_ISDELETED),
							Core.DB.conditions.ComparisonType.EQUAL,
							"0"
						)
					),
					pageOuter
				) select Session.LoadByKey(stringId);
			return new XElement[] {
				new XElement("users",
					from session in sessions
					let account = session.account
					where !account.isStatusHidden
					select account.user.exportToXmlForViewing(
						context,
						new XElement("lastActivity", session.lastHumanActivity.ToXml()),
						!account.isDetailedStatusHidden ? new XElement("lastUrl", session.lastUrl) : null
					)
				)
			};
		}

	}

}