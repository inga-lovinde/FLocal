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

	class UserInfoHandler : AbstractGetHandler {

		override protected string templateName {
			get {
				return "UserInfo.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificData(WebContext context) {
			User user;
			{
				int userId;
				if(int.TryParse(context.requestParts[2], out userId)) {
					user = User.LoadById(userId);
				} else {
					user = User.LoadByName(context.requestParts[2]);
				}
			}
			Account account = null;
			if(context.session != null) {
				try {
					account = Account.LoadByUser(user);
				} catch(NotFoundInDBException) {
				}
			}
			return new XElement[] {
				user.exportToXmlForViewing(context),
				(account == null) ? null : new XElement("accountId", account.id.ToString()), //for PM history, PM send etc
			};
		}

	}

}