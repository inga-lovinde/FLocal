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

	class UserListHandler : AbstractGetHandler {

		override protected string templateName {
			get {
				return "UserList.xslt";
			}
		}

		override protected XElement[] getSpecificData(WebContext context) {
			PageOuter pageOuter = PageOuter.createFromGet(context.requestParts, context.userSettings.usersPerPage);
			IEnumerable<User> users = User.getUsers(pageOuter, context);
			return new XElement[] {
				new XElement("users", 
					from user in users select user.exportToXmlForViewing(context),
					pageOuter.exportToXml(2, 5, 2)
				)
			};
		}

	}

}