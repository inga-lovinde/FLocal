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

	class PMReplyHandler : AbstractNewMessageHandler<FLocal.Common.URL.my.conversations.Reply> {

		override protected string templateName {
			get {
				return "PMReply.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificNewMessageData(WebContext context) {
			PMMessage message = this.url.pm;
			string quoted = UBBParser.StripQuotes(message.bodyUBB).Trim();
			return new XElement[] {
				message.exportToXml(context),
				new XElement("quoted", quoted),
			};
		}
	}

}