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
				case "boardasthread":
					return new handlers.response.BoardAsThreadHandler();
				case "thread":
					return new handlers.ThreadHandler();
				case "post":
					return new handlers.PostHandler();
				case "login":
					return new handlers.response.LoginHandler();
				case "migrateaccount":
					return new handlers.response.MigrateAccountHandler();
				case "static":
					return new handlers.StaticHandler(context.requestParts);
				case "do":
					if(context.requestParts.Length < 2) {
						return new handlers.WrongUrlHandler();
					} else {
						switch(context.requestParts[1].ToLower()) {
							case "migrateaccount":
								return new handlers.request.MigrateAccountHandler();
							default:
								return new handlers.WrongUrlHandler();
						}
					}
				default:
					return new handlers.DebugHandler(context.requestParts[0]);
			}
		}

	}
}
