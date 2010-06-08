using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using FLocal.Core;

namespace FLocal.IISHandler {
	class HandlersFactory {

		public static ISpecificHandler getHandler(WebContext context) {
			if(!context.httprequest.Path.EndsWith("/")) {
				return new handlers.WrongUrlHandler();
//				throw new FLocalException("Malformed url");
			}
			string[] requestParts = context.httprequest.Path.Split("/", StringSplitOptions.RemoveEmptyEntries);
			if(requestParts.Length < 1) return new handlers.RootHandler();
			switch(requestParts[0]) {
				case "boards":
					return new handlers.BoardsHandler();
				default:
					return new handlers.DebugHandler(requestParts[0]);
			}
		}

	}
}
