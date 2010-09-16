using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using FLocal.Common;
using FLocal.Common.dataobjects;
using FLocal.Core;
using FLocal.Core.DB;

namespace FLocal.IISHandler.handlers.response {

	class ConversationsHandler : AbstractGetHandler {

		override protected string templateName {
			get {
				return "Conversations.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificData(WebContext context) {
			PageOuter pageOuter = PageOuter.createFromGet(context.requestParts, context.userSettings.threadsPerPage, 2);
			IEnumerable<PMConversation> conversations = PMConversation.getConversations(context.session.account, pageOuter, pageOuter.descendingDirection);
			XElement[] result = new XElement[] {
				new XElement("conversations", 
					from conversation in conversations select conversation.exportToXml(context, false),
					pageOuter.exportToXml(1, 5, 1)
				)
			};

			return result;
		}

	}

}