using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Common;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.response {

	class SettingsHandler : AbstractGetHandler {

		override protected string templateName {
			get {
				return "Settings.xslt";
			}
		}

		override protected XElement[] getSpecificData(WebContext context) {
			IUserSettings settings = AccountSettings.LoadByAccount(context.session.account);

			return new XElement[] {
				settings.exportToXml(context),
				new XElement("skins", from skin in Skin.allSkins select skin.exportToXml()),
			};
		}
	}

}