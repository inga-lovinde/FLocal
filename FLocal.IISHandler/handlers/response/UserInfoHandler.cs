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

	class UserInfoHandler : AbstractUserGetHandler<FLocal.Common.URL.users.user.Info> {

		override protected string templateName {
			get {
				return "UserInfo.xslt";
			}
		}

		override protected IEnumerable<XElement> getUserSpecificData(WebContext context, User user) {
			return new XElement[] {
				new XElement("punishments", from punishment in user.getPunishments(Diapasone.unlimited) select punishment.exportToXml(context)),
				new XElement("restrictions", from restriction in Restriction.GetRestrictions(user) select restriction.exportToXml(context)),
			};
		}

	}

}