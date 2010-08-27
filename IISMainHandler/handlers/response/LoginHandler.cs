﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core;
using FLocal.Common;
using System.Xml.Linq;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.response {
	class LoginHandler : AbstractGetHandler {

		protected override string templateName {
			get {
				return "Login.xslt";
			}
		}

		protected override IEnumerable<XElement> getSpecificData(WebContext context) {
			return new XElement[] {
				new XElement("isLocalNetwork", LocalNetwork.IsLocalNetwork(context.remoteHost).ToPlainString()),
				new XElement("ip", context.remoteHost.ToString()),
			};
		}

	}
}
