﻿using System;
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

	class UserInfoHandler : AbstractUserGetHandler {

		override protected string templateName {
			get {
				return "UserInfo.xslt";
			}
		}

		override protected IEnumerable<XElement> getUserSpecificData(WebContext context, User user) {
			return new XElement[] {
			};
		}

	}

}