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

			#region robots
			if(context.httprequest.Path.ToLower().StartsWith("/robots.txt")) {
				return new handlers.response.RobotsHandler();
			}
			#endregion

			switch(context.requestParts[0].ToLower()) {
				case "q":
					return new handlers.response.QuickLinkHandler();
				case "allposts":
					return new handlers.response.AllPostsHandler();
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
						case "edit":
							return new handlers.response.EditHandler();
						case "reply":
							return new handlers.response.ReplyHandler();
						case "pmreply":
							return new handlers.response.PMReplyToPostHandler();
						case "punish":
							return new handlers.response.PunishHandler();
						default:
							return new handlers.WrongUrlHandler();
					}
				case "my":
					if(context.requestParts.Length == 1) {
						if(context.account != null) {
							throw new RedirectException("/My/Conversations/");
						} else {
							throw new RedirectException("/My/Login/");
						}
					}
					switch(context.requestParts[1].ToLower()) {
						case "login":
							if(context.requestParts.Length == 2) {
								return new handlers.response.LoginHandler();
							} else {
								switch(context.requestParts[2].ToLower()) {
									case "migrateaccount":
										return new handlers.response.MigrateAccountHandler();
									case "registerbyinvite":
										return new handlers.response.RegisterByInviteHandler();
									default:
										return new handlers.WrongUrlHandler();
								}
							}
						case "settings":
							return new handlers.response.SettingsHandler();
						case "conversations":
							if(context.requestParts.Length == 2) {
								return new handlers.response.ConversationsHandler();
							} else {
								switch(context.requestParts[2].ToLower()) {
									case "conversation":
										return new handlers.response.ConversationHandler();
									case "pmsend":
										return new handlers.response.PMSendHandler();
									case "pmreply":
										return new handlers.response.PMReplyHandler();
									default:
										return new handlers.response.ConversationsHandler();
								}
							}
						default:
							return new handlers.WrongUrlHandler();
					}
				case "users":
					if(context.requestParts.Length == 1) {
						throw new RedirectException("/Users/All/");
					}
					switch(context.requestParts[1].ToLower()) {
						case "all":
							return new handlers.response.UserListHandler();
						case "active":
							return new handlers.response.ActiveAccountListHandler();
						case "online":
							return new handlers.response.WhoIsOnlineHandler();
						case "user":
							if(context.requestParts.Length < 3) {
								return new handlers.WrongUrlHandler();
							}
							if(context.requestParts.Length == 3) {
								return new handlers.response.UserInfoHandler();
							}
							switch(context.requestParts[3].ToLower()) {
								case "posts":
									return new handlers.response.UserPostsHandler();
								case "replies":
									return new handlers.response.UserRepliesHandler();
								case "pollsparticipated":
									return new handlers.response.UserPollsParticipatedHandler();
								default:
									return new handlers.WrongUrlHandler();
							}
						default:
							return new handlers.WrongUrlHandler();
					}
				case "upload":
					if(context.requestParts.Length < 2) {
						throw new RedirectException("/Upload/List/");
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
				case "maintenance":
					if(context.requestParts.Length < 2) {
						return new handlers.WrongUrlHandler();
					}
					switch(context.requestParts[1].ToLower()) {
						case "cleancache":
							return new handlers.response.maintenance.CleanCacheHandler();
						default:
							return new handlers.WrongUrlHandler();
					}
				case "poll":
					if(context.requestParts.Length < 2) {
						return new handlers.WrongUrlHandler();
					}
					switch(context.requestParts[1].ToLower()) {
						case "create":
							return new handlers.response.CreatePollHandler();
						default:
							return new handlers.response.PollHandler();
					}
				case "localnetworks":
					return new handlers.response.LocalNetworksListHandler();
				case "static":
					return new handlers.StaticHandler(context.requestParts);
				case "registerbyinvite":
					string[] rbi_parts = context.requestParts;
					rbi_parts[0] = "My/Login/RegisterByInvite";
					throw new RedirectException("/" + string.Join("/", rbi_parts));
				case "user":
					string[] u_parts = context.requestParts;
					u_parts[0] = "Users/User";
					throw new RedirectException("/" + string.Join("/", u_parts));
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
							case "register":
								return new handlers.request.RegisterHandler();
							case "registerbyinvite":
								return new handlers.request.RegisterByInviteHandler();
							case "edit":
								return new handlers.request.EditHandler();
							case "punish":
								return new handlers.request.PunishHandler();
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
							case "newpoll":
								return new handlers.request.CreatePollHandler();
							case "vote":
								return new handlers.request.VoteHandler();
							case "maintenance":
								if(context.requestParts.Length < 3) {
									return new handlers.WrongUrlHandler();
								}
								switch(context.requestParts[2].ToLower()) {
									case "cleancache":
										return new handlers.request.maintenance.CleanCacheHandler();
									default:
										return new handlers.WrongUrlHandler();
								}
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
