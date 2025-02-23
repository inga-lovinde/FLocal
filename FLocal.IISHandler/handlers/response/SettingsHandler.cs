﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Web.Core;
using FLocal.Common;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.response {

	class SettingsHandler : AbstractGetHandler<FLocal.Common.URL.my.Settings> {

		override protected string templateName {
			get {
				return "Settings.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificData(WebContext context) {
			IUserSettings settings = AccountSettings.LoadByAccount(context.session.account);

			return new XElement[] {
				settings.exportToXml(context),
				new XElement("skins", from skin in Skin.allSkins select skin.exportToXml()),
				new XElement("modernSkins", from modernSkin in ModernSkin.allSkins select modernSkin.exportToXml()),
				new XElement("machicharas", from machichara in Machichara.allMachicharas select machichara.exportToXml())
			};
		}
	}

}