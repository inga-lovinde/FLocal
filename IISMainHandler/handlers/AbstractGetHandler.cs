using System;
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

		abstract protected IEnumerable<XElement> getSpecificData(WebContext context);

		protected IEnumerable<XElement> getCommonData(WebContext context) {
			return new XElement[] {
				new XElement("title", Config.instance.AppInfo),
				new XElement("current", DateTime.Now.ToXml()),
				context.exportSession(),
				context.userSettings.skin.exportToXml(),
				new XElement("currentUrl", "/" + String.Join("/", context.requestParts) + "/"),
			};
		}

		private XDocument getData(WebContext context) {
			return new XDocument(
				new XElement("root",
					this.getSpecificData(context),
					this.getCommonData(context)
				)
			);
		}

		public void Handle(WebContext context) {
			try {
				context.httpresponse.Write(context.Transform(this.templateName, this.getData(context)));
			} catch(Exception e) {
				context.LogError(e);
				context.httpresponse.Write(context.Transform("Exception.xslt", new XDocument(new XElement("root", this.getCommonData(context), e.ToXml()))));
			}
		}

	}
}
