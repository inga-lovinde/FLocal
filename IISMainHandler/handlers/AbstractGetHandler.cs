﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Common;

namespace FLocal.IISHandler.handlers {
	abstract class AbstractGetHandler : ISpecificHandler {
		
		abstract protected string templateName {
			get;
		}

		abstract protected XElement[] getSpecificData(WebContext context);

		private XDocument getData(WebContext context) {
			return new XDocument(
				new XElement("root",
					this.getSpecificData(context),
					new XElement("title", Config.instance.AppInfo),
					context.exportSession(),
					context.userSettings.skin.exportToXml()
				)
			);
		}

		public void Handle(WebContext context) {
			context.httpresponse.Write(context.Transform(this.templateName, this.getData(context)));
		}

	}
}
