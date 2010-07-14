using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Common;

namespace FLocal.IISHandler.handlers.response {
	abstract class AbstractNewMessageHandler : AbstractGetHandler {

		abstract protected IEnumerable<XElement> getSpecificNewMessageData(WebContext context);

		protected override IEnumerable<XElement> getSpecificData(WebContext context) {
			var result = new List<XElement>();
			if(context.httprequest.Form.AllKeys.Contains("title")) {
				result.Add(new XElement("title", context.httprequest.Form["title"]));
			}
			if(context.httprequest.Form.AllKeys.Contains("Body")) {
				result.Add(new XElement("bodyUBB", context.httprequest.Form["Body"]));
				result.Add(new XElement("bodyIntermediate", context.outputParams.preprocessBodyIntermediate(UBBParser.UBBToIntermediate(context.httprequest.Form["Body"]))));
			}
			return result.Union(this.getSpecificNewMessageData(context));
		}

	}
}
