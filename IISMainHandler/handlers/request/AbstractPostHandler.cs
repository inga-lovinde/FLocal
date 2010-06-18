using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Common;

namespace FLocal.IISHandler.handlers.request {
	abstract class AbstractPostHandler : ISpecificHandler {

		abstract protected string templateName {
			get;
		}

		virtual protected bool shouldBeLoggedIn {
			get {
				return true;
			}
		}

		virtual protected bool shouldBeGuest {
			get {
				return false;
			}
		}

		abstract protected XElement[] Do(WebContext context);

		private XDocument getData(WebContext context) {
			return new XDocument(
				new XElement("root",
					new XElement("title", Config.instance.AppInfo),
					this.Do(context),
					context.exportSession()
				)
			);
		}

		public void Handle(WebContext context) {

			Uri referer = context.httprequest.UrlReferrer;
			if(referer == null || referer.Host != context.httprequest.Url.Host) {
				throw new System.Web.HttpException(403, "Wrong referer");
			}

			if(this.shouldBeGuest && context.session != null) throw new FLocalException("Should be guest");
			if(this.shouldBeLoggedIn && context.session == null) throw new FLocalException("Should be anonymous");
			context.httpresponse.Write(context.Transform(this.templateName, this.getData(context)));
		}

	}
}
