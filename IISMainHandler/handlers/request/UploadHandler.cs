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

namespace FLocal.IISHandler.handlers.request {
	class UploadHandler : AbstractPostHandler {

		protected override string templateName {
			get {
				return "result/Upload.xslt";
			}
		}

		protected override XElement[] Do(WebContext context) {
			HttpPostedFile file = context.httprequest.Files["file"];
			if(file == null) throw new FLocalException("file not uploaded");
			if(file.ContentLength != file.InputStream.Length) throw new FLocalException("file is not uploaded completely");
			Upload upload;
			try {
				upload = UploadManager.UploadFile(file.InputStream, System.IO.Path.GetFileName(file.FileName), DateTime.Now, context.session.account.user, null);
			} catch(UploadManager.AlreadyUploadedException e) {
				upload = Upload.LoadById(e.uploadId);
			}
			return new XElement[] {
				new XElement("uploadedId", upload.id)
			};
		}

	}
}
