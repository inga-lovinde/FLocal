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

	class PMSendHandler : AbstractGetHandler {

		override protected string templateName {
			get {
				return "PMSend.xslt";
			}
		}

		override protected XElement[] getSpecificData(WebContext context) {
			if(context.requestParts.Length > 1) {
				Account account = Account.LoadById(int.Parse(context.requestParts[1]));
				return new XElement[] {
					new XElement("receiver", account.exportToXml(context)),
				};
			}
			return new XElement[0];
		}
	}

}