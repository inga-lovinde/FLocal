using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Common;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.request {
	
	class SendPMHandler : AbstractNewMessageHandler {

		protected override string templateName {
			get {
				return "result/PMSent.xslt";
			}
		}

		protected override XElement[] Do(WebContext context) {
			Account receiver;
			if(context.httprequest.Form.AllKeys.Contains("receiverId")) {
				receiver = Account.LoadById(int.Parse(context.httprequest.Form["receiverId"]));
			} else if(context.httprequest.Form.AllKeys.Contains("receiverName")) {
				receiver = Account.LoadByUser(User.LoadByName(context.httprequest.Form["receiverName"]));
			} else {
				throw new ApplicationException("receiverId/receiverName not passed");
			}

			if(Config.instance.IsMigrationEnabled && receiver.needsMigration) throw new ApplicationException("User is not migrated");

			PMMessage newMessage = PMConversation.SendPMMessage(
				context.account,
				receiver,
				this.getTitle(context),
				this.getBody(context)
			);
			
			newMessage.conversation.markAsRead(context.session.account, newMessage, newMessage);

			return new XElement[] { newMessage.exportToXml(context) };
		}

	}
}
