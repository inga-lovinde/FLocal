using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Common.dataobjects;
using FLocal.Importer;
using System.Text.RegularExpressions;
using FLocal.Core;
using FLocal.Common;
using FLocal.Common.actions;
using System.Web;

namespace FLocal.IISHandler.handlers.request.avatars {
	class SetAsDefaultHandler : AbstractPostHandler {

		protected override string templateName {
			get {
				return "result/AvatarSetted.xslt";
			}
		}

		protected override XElement[] Do(WebContext context) {

			Upload upload = null;

			if(!string.IsNullOrEmpty(context.httprequest.Form["uploadId"])) {
				upload = Upload.LoadById(int.Parse(context.httprequest.Form["uploadId"]));
			}
			
			context.account.user.SetAvatar(upload);

			return new XElement[] {
				(upload != null) ? new XElement("uploadedId", upload.id) : null
			};
		}

	}
}
