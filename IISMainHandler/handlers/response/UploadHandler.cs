using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core;
using FLocal.Common;
using System.Xml.Linq;
using FLocal.Common.dataobjects;
using FLocal.Core.DB;
using FLocal.Core.DB.conditions;

namespace FLocal.IISHandler.handlers.response {
	class UploadHandler : RedirectGetHandler<FLocal.Common.URL.upload.Item> {

		protected override string templateName {
			get {
				return null;
			}
		}

		protected override string getRedirectUrl(WebContext context) {
			Uri referer = context.httprequest.UrlReferrer;
			if(referer == null || referer.Host != context.httprequest.Url.Host) {
				//throw new AccessDeniedException();
				return new Common.URL.upload.Info(this.url.upload.id.ToString(), null).canonical;
			}

			string mime = Util.getMimeByExtension(this.url.upload.extension);
			if(mime != null) {
				context.httpresponse.ContentType = mime;
			} else {
				//throw new HttpException(403, "wrong file type");
				throw new WrongUrlException();
			}
			context.httpresponse.AddHeader("content-disposition", "attachment; filename=" + this.url.upload.filename);

			context.httpresponse.Cache.SetExpires(DateTime.Now.AddDays(10));
			context.httpresponse.Cache.SetLastModified(this.url.upload.uploadDate);
			context.httpresponse.Cache.SetCacheability(System.Web.HttpCacheability.Public);

			UploadManager.WriteUpload(this.url.upload, context.httpresponse.OutputStream);

			throw new response.SkipXsltTransformException();
		}

	}
}
