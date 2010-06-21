using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core;
using FLocal.Common;
using System.Xml.Linq;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.response {
	class UploadHandler : AbstractGetHandler {

		protected override string templateName {
			get {
				return null;
			}
		}

		protected override System.Xml.Linq.XElement[] getSpecificData(WebContext context) {
			if(context.requestParts.Length != 2) throw new FLocalException("wrong url");
			Upload upload = Upload.LoadById(int.Parse(context.requestParts[1]));
			throw new RedirectException(Config.instance.UploaderUrl + "Data/" + upload.hash + "." + upload.extension);
		}

	}
}
