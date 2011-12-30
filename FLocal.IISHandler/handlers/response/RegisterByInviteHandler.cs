using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Web.Core;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.response {
	class RegisterByInviteHandler : AbstractGetHandler<FLocal.Common.URL.my.login.RegisterByInvite> {

		protected override string templateName {
			get {
				return "RegisterByInvite.xslt";
			}
		}

		protected override IEnumerable<XElement> getSpecificData(WebContext context) {
			string[] parts = this.url.remainder.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
			int inviteId = int.Parse(parts[0]);
			string code = parts[1];
			Invite invite = Invite.LoadById(inviteId);
			if(invite.isUsed) throw new FLocalException("Invite is already used");
			if(invite.code != code) throw new FLocalException("Code mismatch");
			return new XElement[] {
				invite.exportToXml(context),
			};
		}

	}
}
