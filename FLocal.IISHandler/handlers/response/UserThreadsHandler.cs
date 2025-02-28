﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using FLocal.Common;
using FLocal.Common.dataobjects;
using Web.Core;
using Web.Core.DB;
using Web.Core.DB.conditions;

namespace FLocal.IISHandler.handlers.response {

	class UserThreadsHandler : AbstractUserGetHandler<FLocal.Common.URL.users.user.Threads> {

		override protected string templateName {
			get {
				return "UserThreads.xslt";
			}
		}

		override protected IEnumerable<XElement> getUserSpecificData(WebContext context, User user) {
			PageOuter pageOuter = PageOuter.createFromUrl(this.url, context.userSettings.postsPerPage);
			IEnumerable<Thread> threads = user.getThreads(pageOuter, pageOuter.descendingDirection);

			return new XElement[] {
				new XElement("threads",
					from thread in threads select thread.exportToXml(context),
					pageOuter.exportToXml(1, 5, 1)
				)
			};
		}

	}

}