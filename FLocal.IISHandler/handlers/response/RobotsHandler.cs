using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using Web.Core;
using FLocal.Common;

namespace FLocal.IISHandler.handlers.response {
	class RobotsHandler : AbstractGetHandler<FLocal.Common.URL.Robots> {

		protected override string templateName {
			get {
				return null;
			}
		}

		protected override IEnumerable<System.Xml.Linq.XElement> getSpecificData(WebContext context) {
			context.httpresponse.ContentType = "text/plain";
			if(Config.instance.IsIndexingDisabled) {
				context.httpresponse.WriteLine("User-agent: *");
				context.httpresponse.WriteLine("Disallow: /");
				foreach(var subnet in context.remoteHost.matchingSubnets) {
					context.httpresponse.WriteLine(subnet.ToString());
				}
			}
			throw new SkipXsltTransformException();
		}

	}
}
