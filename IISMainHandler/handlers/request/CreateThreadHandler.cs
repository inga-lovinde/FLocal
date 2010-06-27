using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FLocal.IISHandler.handlers.request {
	class CreateThreadHandler : AbstractNewMessageHandler {

		protected override string templateName {
			get {
				throw new NotImplementedException();
			}
		}

		protected override XElement[] Do(WebContext context) {
			throw new NotImplementedException();
		}
	}
}
