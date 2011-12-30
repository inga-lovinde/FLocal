using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Common.dataobjects;
using FLocal.Migration.Gateway;
using System.Text.RegularExpressions;
using Web.Core;
using FLocal.Common;
using FLocal.Common.actions;
using System.Web;

namespace FLocal.IISHandler.handlers.request.avatars {
	class RemoveHandler : AbstractPostHandler {

		protected override string templateName {
			get {
				return "result/AvatarRemoved.xslt";
			}
		}

		protected override XElement[] Do(WebContext context) {

			Upload upload = Upload.LoadById(int.Parse(context.httprequest.Form["uploadId"]));
			
			AvatarsSettings.RemoveAvatar(context.account, upload);

			return new XElement[] {
				new XElement("uploadedId", upload.id)
			};
		}

	}
}
