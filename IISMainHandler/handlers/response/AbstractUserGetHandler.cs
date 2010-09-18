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
using FLocal.Core.DB.conditions;

namespace FLocal.IISHandler.handlers.response {

	abstract class AbstractUserGetHandler<TUrl> : AbstractGetHandler<TUrl> where TUrl : FLocal.Common.URL.users.user.Abstract {

		abstract protected IEnumerable<XElement> getUserSpecificData(WebContext context, User user);

		sealed override protected IEnumerable<XElement> getSpecificData(WebContext context) {
			User user = this.url.user;
			Account account = null;
			if(context.session != null) {
				try {
					account = Account.LoadByUser(user);
				} catch(NotFoundInDBException) {
				}
			}
			Session lastSession = null;
			if(account != null && !account.isDetailedStatusHidden) {
				try {
					lastSession = Session.GetLastSession(account);
				} catch(NotFoundInDBException) {
				}
			}
			return new XElement[] {
				user.exportToXmlForViewing(context),
				(account == null) ? null : new XElement("accountId", account.id.ToString()), //for PM history, PM send etc
				(lastSession == null) ? null : new XElement("lastActivity", lastSession.lastHumanActivity.ToXml()),
			}.Concat(this.getUserSpecificData(context, user));
		}

	}

}