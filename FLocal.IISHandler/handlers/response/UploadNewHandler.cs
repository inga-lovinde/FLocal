using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Core;
using FLocal.Common;
using System.Xml.Linq;
using FLocal.Common.dataobjects;
using Web.Core.DB;
using Web.Core.DB.conditions;

namespace FLocal.IISHandler.handlers.response {
	class UploadNewHandler : AbstractGetHandler<FLocal.Common.URL.upload.New> {

		protected override string templateName {
			get {
				return "UploadNew.xslt";
			}
		}

		protected override IEnumerable<XElement> getSpecificData(WebContext context) {
			return new XElement[0];
		}

	}
}
