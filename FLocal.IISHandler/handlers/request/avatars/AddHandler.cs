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
	class AddHandler : AbstractPostHandler {

		protected override string templateName {
			get {
				return "result/AvatarAdded.xslt";
			}
		}

		protected override XElement[] Do(WebContext context) {
			Upload upload;
			if(context.httprequest.Files["file"] != null && context.httprequest.Files["file"].ContentLength > 0) {
				HttpPostedFile file = context.httprequest.Files["file"];
				if(file.ContentLength != file.InputStream.Length) throw new FLocalException("file is not uploaded completely");
				upload = UploadManager.SafeUploadFile(file.InputStream, System.IO.Path.GetFileName(file.FileName), context.session.account.user);
			} else {
				upload = Upload.LoadById(int.Parse(context.httprequest.Form["uploadId"]));
			}
			
			AvatarsSettings.AddAvatar(context.account, upload);

			return new XElement[] {
				new XElement("uploadedId", upload.id)
			};
		}

	}
}
