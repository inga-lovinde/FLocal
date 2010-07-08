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
				return "UserList.xslt";
			}
		}

		override protected XElement[] getSpecificData(WebContext context) {
			PageOuter pageOuter = PageOuter.createUnlimited(context.userSettings.usersPerPage);
			IEnumerable<Session> sessions = 
				from stringId in Config.instance.mainConnection.LoadIdsByConditions(
					Session.TableSpec.instance,
					new Core.DB.conditions.ComparisonCondition(
						Session.TableSpec.instance.getColumnSpec(Session.TableSpec.FIELD_LASTACTIVITY),
						Core.DB.conditions.ComparisonType.GREATEROREQUAL,
						DateTime.Now.Subtract(Config.instance.ActivityThreshold).ToUTCString()
					),
					pageOuter
				) select Session.LoadByKey(stringId);
			return new XElement[] {
				new XElement("users", 
					from session in sessions select session.account.user.exportToXmlForViewing(context),
					pageOuter.exportToXml(2, 5, 2)
				)
			};
		}

	}

}