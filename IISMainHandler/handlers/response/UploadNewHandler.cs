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
	class UploadNewHandler : AbstractGetHandler {

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
