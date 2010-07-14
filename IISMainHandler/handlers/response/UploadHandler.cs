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
	class UploadHandler : AbstractGetHandler {

		protected override string templateName {
			get {
				return null;
			}
		}

		protected override IEnumerable<XElement> getSpecificData(WebContext context) {
			Upload upload = Upload.LoadById(int.Parse(context.requestParts[2]));
			throw new RedirectException(Config.instance.UploaderUrl + "Data/" + upload.hash + "." + upload.extension);
		}

	}
}
