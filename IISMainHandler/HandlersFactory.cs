using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using FLocal.Core;

namespace FLocal.IISHandler {
	class HandlersFactory {

		public static ISpecificHandler getHandler(WebContext context) {
//			if(!context.httprequest.Path.EndsWith("/")) {
//				return new handlers.WrongUrlHandler();
//				throw new FLocalException("Malformed url");
//			}
			if(context.requestParts.Length < 1) return new handlers.RootHandler();
			switch(context.requestParts[0].ToLower()) {
				case "boards":
					return new handlers.BoardsHandler();
				case "board":
					return new handlers.BoardHandler();
				case "static":
					return new handlers.StaticHandler(context.requestParts);
				default:
					return new handlers.DebugHandler(context.requestParts[0]);
			}
		}

	}
}
