using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Common;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.response {

	class PMReplyHandler : AbstractGetHandler {

		override protected string templateName {
			get {
				return "PMReply.xslt";
			}
		}

		override protected XElement[] getSpecificData(WebContext context) {
			PMMessage message = PMMessage.LoadById(int.Parse(context.requestParts[1]));

			return new XElement[] {
				message.exportToXml(context)
			};
		}
	}

}