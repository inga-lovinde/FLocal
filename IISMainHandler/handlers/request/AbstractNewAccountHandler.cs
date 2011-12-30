using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Common.dataobjects;
using FLocal.Migration.Gateway;
using System.Text.RegularExpressions;
using Web.Core;
using FLocal.Common;
using FLocal.Common.actions;

namespace FLocal.IISHandler.handlers.request {
	abstract class AbstractNewAccountHandler : AbstractPostHandler {

		protected override string templateName {
			get {
				return "result/NewAccount.xslt";
			}
		}

		protected override bool shouldBeLoggedIn {
			get { return false; }
		}

		sealed protected override bool shouldBeGuest {
			get { return true; }
		}

		abstract protected Account DoCreateAccount(WebContext context);

		sealed protected override XElement[] Do(WebContext context) {

			if(context.httprequest.Form["constitution"] != "constitution") {
				throw new FLocalException("constitution not accepted");
			}
			if(context.httprequest.Form["showPostsToAll"] != "showPostsToAll") {
				throw new FLocalException("publicity not accepted");
			}
			if(context.httprequest.Form["law"] != "law") {
				throw new FLocalException("laws not accepted");
			}

			this.DoCreateAccount(context);
			return new XElement[0];
		}

	}
}
