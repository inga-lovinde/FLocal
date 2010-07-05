using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using FLocal.Common;
using FLocal.Common.dataobjects;
using FLocal.Core.DB;

namespace FLocal.IISHandler.handlers.response {

	class ActiveAccountListHandler : AbstractGetHandler {

		override protected string templateName {
			get {
				return "UserList.xslt";
			}
		}

		override protected XElement[] getSpecificData(WebContext context) {
			//PageOuter pageOuter = PageOuter.createFromGet(context.requestParts, context.userSettings.usersPerPage, 1);
			PageOuter pageOuter = PageOuter.createUnlimited(context.userSettings.usersPerPage);
			IEnumerable<Account> accounts = Account.LoadByIds(
				from stringId in Config.instance.mainConnection.LoadIdsByConditions(
					Account.TableSpec.instance,
					new Core.DB.conditions.ComparisonCondition(
						Account.TableSpec.instance.getColumnSpec(Account.TableSpec.FIELD_NEEDSMIGRATION),
						Core.DB.conditions.ComparisonType.EQUAL,
						"0"
					),
					pageOuter
				) select int.Parse(stringId)
			);
			return new XElement[] {
				new XElement("users", 
					from account in accounts select account.user.exportToXmlForViewing(context),
					pageOuter.exportToXml(2, 5, 2)
				)
			};
		}

	}

}