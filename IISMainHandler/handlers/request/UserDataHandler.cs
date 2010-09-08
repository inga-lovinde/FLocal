using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.request {
	class UserDataHandler : AbstractPostHandler {

		protected override string templateName {
			get {
				return "result/SettingsSaved.xslt";
			}
		}

		protected override XElement[] Do(WebContext context) {

			User.UserData newData = new User.UserData {
				location = context.httprequest.Form["location"],
				title = context.httprequest.Form["title"],
				signatureUbb = context.httprequest.Form["signature"],
				biographyUbb = context.httprequest.Form["biography"],
			};

			context.account.user.UpdateData(newData);

			return new XElement[0];
		}

	}
}
