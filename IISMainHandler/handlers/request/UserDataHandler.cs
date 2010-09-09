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
				location = context.httprequest.Form["location"].Trim(),
				title = context.httprequest.Form["title"].Trim(),
				signatureUbb = context.httprequest.Form["signature"].Trim(),
				biographyUbb = context.httprequest.Form["biography"].Trim(),
			};

			context.account.user.UpdateData(newData);

			return new XElement[0];
		}

	}
}
