using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FLocal.IISHandler {
	class HandlersFactory {

		public static ISpecificHandler getHandler(WebContext context) {
			string[] requestParts = context.httprequest.Path.Split("/", StringSplitOptions.RemoveEmptyEntries);
			if(requestParts.Length < 1) return new handlers.RootHandler();
			return new handlers.DebugHandler(requestParts[0]);
			//return new handlers.WrongUrlHandler();
		}

	}
}
