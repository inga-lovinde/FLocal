using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Web.Core;
using FLocal.Common;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.response {

	class PMSendHandler : AbstractNewMessageHandler<FLocal.Common.URL.my.conversations.NewPM> {

		override protected string templateName {
			get {
				return "PMSend.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificNewMessageData(WebContext context) {
			if(!string.IsNullOrEmpty(this.url.remainder)) {
				Account receiver = Account.LoadById(int.Parse(this.url.remainder));
				if(receiver.needsMigration) throw new ApplicationException("User is not migrated");
				return new XElement[] {
					new XElement("receiver", receiver.exportToXml(context)),
				};
			}
			return new XElement[0];
		}
	}

}