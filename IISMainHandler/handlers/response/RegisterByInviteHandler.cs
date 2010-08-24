using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.response {
	class RegisterByInviteHandler : AbstractGetHandler {

		protected override string templateName {
			get {
				return "RegisterByInvite.xslt";
			}
		}

		protected override IEnumerable<XElement> getSpecificData(WebContext context) {
			int inviteId = int.Parse(context.requestParts[1]);
			string code = context.requestParts[2];
			Invite invite = Invite.LoadById(inviteId);
			if(invite.isUsed) throw new FLocalException("Invite is already used");
			if(invite.code != code) throw new FLocalException("Code mismatch");
			return new XElement[] {
				invite.exportToXml(context),
			};
		}

	}
}
