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
				try {
					upload = UploadManager.UploadFile(file.InputStream, System.IO.Path.GetFileName(file.FileName), DateTime.Now, context.session.account.user, null);
				} catch(UploadManager.AlreadyUploadedException e) {
					upload = Upload.LoadById(e.uploadId);
				}
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
