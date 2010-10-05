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
	class UploadInfoHandler : AbstractGetHandler<FLocal.Common.URL.upload.Info> {

		protected override string templateName {
			get {
				return "UploadInfo.xslt";
			}
		}

		protected override IEnumerable<XElement> getSpecificData(WebContext context) {
			if(context.session == null) {
				throw new AccessDeniedException();
			}
			return new XElement[] {
				this.url.upload.exportToXml(context),
			};
		}

	}
}
