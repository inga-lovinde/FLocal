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

		abstract protected IEnumerable<XElement> getSpecificData(WebContext context);

		protected IEnumerable<XElement> getCommonData(WebContext context) {
			return new XElement[] {
				new XElement(
					"url",
					new XElement("host", context.httprequest.Url.Host),
					new XElement("port", context.httprequest.Url.Port)
				),
				new XElement("handlerName", this.GetType().FullName),
				new XElement("title", Config.instance.AppInfo),
				new XElement("current", DateTime.Now.ToXml()),
				context.exportSession(),
				context.userSettings.skin.exportToXml(),
				new XElement("currentUrl", "/" + String.Join("/", context.requestParts) + "/"),
				context.exportRequestParameters(),
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
				context.WriteTransformResult(this.templateName, this.getData(context));
			} catch(RedirectException) {
				throw;
			} catch(WrongUrlException) {
				throw;
			} catch(Exception e) {
				context.LogError(e);
				context.WriteTransformResult("Exception.xslt", new XDocument(new XElement("root", this.getCommonData(context), e.ToXml())));
			}
		}

	}
}
