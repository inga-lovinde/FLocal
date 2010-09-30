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
				throw new AccessViolationException();
			}

			return Config.instance.UploaderUrl + "Data/" + this.url.upload.hash + "." + this.url.upload.extension;
		}

	}
}
