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

	class PMSendHandler : AbstractNewMessageHandler {

		override protected string templateName {
			get {
				return "PMSend.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificNewMessageData(WebContext context) {
			if(context.requestParts.Length > 1) {
				Account receiver = Account.LoadById(int.Parse(context.requestParts[1]));
				if(receiver.needsMigration) throw new ApplicationException("User is not migrated");
				return new XElement[] {
					new XElement("receiver", receiver.exportToXml(context)),
				};
			}
			return new XElement[0];
		}
	}

}