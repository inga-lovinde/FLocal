using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.request {
	class SettingsHandler : AbstractPostHandler {

		protected override string templateName {
			get {
				return "result/SettingsSaved.xslt";
			}
		}

		protected override XElement[] Do(WebContext context) {
			string currentPassword = context.httprequest.Form["currentPassword"];
			string newPassword = context.httprequest.Form["newPassword"];
			if(newPassword != context.httprequest.Form["newPassword2"]) throw new FLocalException("new passwords mismatch");
			string registrationEmail = context.httprequest.Form["registrationEmail"];
			int postsPerPage = int.Parse(context.httprequest.Form["postsPerPage"]);
			int threadsPerPage = int.Parse(context.httprequest.Form["threadsPerPage"]);
			int usersPerPage = int.Parse(context.httprequest.Form["usersPerPage"]);
			int uploadsPerPage = int.Parse(context.httprequest.Form["uploadsPerPage"]);
			Skin skin = Skin.LoadById(int.Parse(context.httprequest.Form["skinId"]));
			Machichara machichara = Machichara.LoadById(int.Parse(context.httprequest.Form["machicharaId"]));

			if((postsPerPage < 1) || (postsPerPage > 200)) throw new FLocalException("wrong number for postsPerPage");
			if((threadsPerPage < 1) || (threadsPerPage > 200)) throw new FLocalException("wrong number for threadsPerPage");
			if((usersPerPage < 1) || (usersPerPage > 200)) throw new FLocalException("wrong number for usersPerPage");
			if((uploadsPerPage < 1) || (uploadsPerPage > 200)) throw new FLocalException("wrong number for uploadsPerPage");

			if(!context.account.checkPassword(currentPassword)) throw new FLocalException("Wrong password. You should enter your current password in order to change settings.");

			AccountSettings.Save(context.session.account, postsPerPage, threadsPerPage, usersPerPage, uploadsPerPage, skin, machichara);

			if(newPassword != null && newPassword != "") {
				context.account.updatePassword(newPassword);
			}

			if(registrationEmail != null && registrationEmail != "") {
				context.account.updateRegistrationEmail(registrationEmail);
			}

			return new XElement[0];
		}

	}
}
