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

	class AvatarsSettingsHandler : AbstractGetHandler<FLocal.Common.URL.my.Avatars> {

		override protected string templateName {
			get {
				return "AvatarsSettings.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificData(WebContext context) {
			
			AvatarsSettings settings = null;
			try {
				settings = AvatarsSettings.LoadByAccount(context.account);
				int accountId = settings.accountId; //just to make sure it is loaded
			} catch(NotFoundInDBException) {
			}

			return new XElement[] {
				(settings != null) ? new XElement("avatars", from avatar in settings.avatars select avatar.exportToXml(context)) : null,
				(context.account.user.avatarId.HasValue) ? new XElement("currentAvatar", context.account.user.avatar.exportToXml(context)) : null,
			};
		}
	}

}