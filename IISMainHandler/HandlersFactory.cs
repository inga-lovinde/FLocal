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

			#region legacy
			if(context.httprequest.Path.ToLower().StartsWith("/user/upload/")) {
				return new handlers.response.LegacyUploadHandler();
			}
			if(context.httprequest.Path.EndsWith(".php")) {
				return new handlers.response.LegacyPHPHandler();
			}
			if(context.httprequest.Path.ToLower().StartsWith("/images/graemlins/")) {
				throw new RedirectException("/static/smileys/" + context.requestParts[2]);
			}
			#endregion

			switch(context.requestParts[0].ToLower()) {
				case "q":
					return new handlers.response.QuickLinkHandler();
				case "boards":
					return new handlers.BoardsHandler();
				case "board":
					if(context.requestParts.Length < 2) {
						return new handlers.WrongUrlHandler();
					}
					if(context.requestParts.Length == 2) {
						return new handlers.BoardHandler();
					}
					switch(context.requestParts[2].ToLower()) {
						case "newthread":
							return new handlers.response.CreateThreadHandler();
						default:
							return new handlers.BoardHandler();
					}
				case "boardasthread":
					return new handlers.response.BoardAsThreadHandler();
				case "thread":
					return new handlers.ThreadHandler();
				case "post":
					if(context.requestParts.Length < 2) {
						return new handlers.WrongUrlHandler();
					}
					if(context.requestParts.Length == 2) {
						return new handlers.PostHandler();
					}
					switch(context.requestParts[2].ToLower()) {
						case "reply":
							return new handlers.response.ReplyHandler();
						case "pmreply":
							return new handlers.response.PMReplyToPostHandler();
						default:
							return new handlers.WrongUrlHandler();
					}
				case "login":
					return new handlers.response.LoginHandler();
				case "migrateaccount":
					return new handlers.response.MigrateAccountHandler();
				case "users":
					return new handlers.response.UserListHandler();
				case "user":
					if(context.requestParts.Length < 2) {
						return new handlers.WrongUrlHandler();
					}
					if(context.requestParts.Length == 2) {
						return new handlers.response.UserInfoHandler();
					}
					switch(context.requestParts[2].ToLower()) {
						case "posts":
							return new handlers.response.UserPostsHandler();
						default:
							return new handlers.WrongUrlHandler();
					}
				case "settings":
					return new handlers.response.SettingsHandler();
				case "conversations":
					return new handlers.response.ConversationsHandler();
				case "conversation":
					return new handlers.response.ConversationHandler();
				case "pmsend":
					return new handlers.response.PMSendHandler();
				case "pmreply":
					return new handlers.response.PMReplyHandler();
				case "upload":
					if(context.requestParts.Length < 2) {
						return new handlers.WrongUrlHandler();
					}
					switch(context.requestParts[1].ToLower()) {
						case "item":
							return new handlers.response.UploadHandler();
						case "new":
							return new handlers.response.UploadNewHandler();
						case "list":
							return new handlers.response.UploadListHandler();
						default:
							return new handlers.WrongUrlHandler();
					}
				case "static":
					return new handlers.StaticHandler(context.requestParts);
				case "do":
					if(context.requestParts.Length < 2) {
						return new handlers.WrongUrlHandler();
					} else {
						switch(context.requestParts[1].ToLower()) {
							case "login":
								return new handlers.request.LoginHandler();
							case "logout":
								return new handlers.request.LogoutHandler();
							case "migrateaccount":
								return new handlers.request.MigrateAccountHandler();
							case "reply":
								return new handlers.request.ReplyHandler();
							case "newthread":
								return new handlers.request.CreateThreadHandler();
							case "settings":
								return new handlers.request.SettingsHandler();
							case "sendpm":
								return new handlers.request.SendPMHandler();
							case "markthreadasread":
								return new handlers.request.MarkThreadAsReadHandler();
							case "upload":
								return new handlers.request.UploadHandler();
							default:
								return new handlers.WrongUrlHandler();
						}
					}
				default:
					return new handlers.WrongUrlHandler();
			}
		}

	}
}
