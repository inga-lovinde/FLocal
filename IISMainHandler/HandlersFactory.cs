using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using FLocal.Core;
using FLocal.Common.URL;
using URL = FLocal.Common.URL;

namespace FLocal.IISHandler {
	class HandlersFactory {

		private static readonly Dictionary<Type, Func<AbstractUrl, ISpecificHandler>> handlersDictionary = new Dictionary<Type,Func<AbstractUrl,ISpecificHandler>> {
			{ typeof(URL.forum.AllPosts),CreateHandler<URL.forum.AllPosts, handlers.response.AllPostsHandler> },
			{ typeof(URL.forum.AllThreads), CreateHandler<URL.forum.AllThreads, handlers.response.AllThreadsHandler> },
			{ typeof(URL.forum.board.Headlines), CreateHandler<URL.forum.board.Headlines, handlers.response.BoardAsThreadHandler> },
			{ typeof(URL.forum.board.NewThread), CreateHandler<URL.forum.board.NewThread, handlers.response.CreateThreadHandler> },
			{ typeof(URL.forum.board.thread.post.Edit), CreateHandler<URL.forum.board.thread.post.Edit, handlers.response.EditHandler> },
			{ typeof(URL.forum.board.thread.post.PMReply), CreateHandler<URL.forum.board.thread.post.PMReply, handlers.response.PMReplyToPostHandler> },
			{ typeof(URL.forum.board.thread.post.Punish), CreateHandler<URL.forum.board.thread.post.Punish, handlers.response.PunishHandler> },
			{ typeof(URL.forum.board.thread.post.Reply), CreateHandler<URL.forum.board.thread.post.Reply, handlers.response.ReplyHandler> },
			{ typeof(URL.forum.board.thread.post.Show), CreateHandler<URL.forum.board.thread.post.Show, handlers.PostHandler> },
			{ typeof(URL.forum.board.thread.Posts), CreateHandler<URL.forum.board.thread.Posts, handlers.ThreadHandler> },
			{ typeof(URL.forum.board.Threads), CreateHandler<URL.forum.board.Threads, handlers.BoardHandler> },
			{ typeof(URL.forum.Boards), CreateHandler<URL.forum.Boards, handlers.BoardsHandler> },
			{ typeof(URL.maintenance.CleanCache), CreateHandler<URL.maintenance.CleanCache, handlers.response.maintenance.CleanCacheHandler> },
			{ typeof(URL.maintenance.LocalNetworks), CreateHandler<URL.maintenance.LocalNetworks, handlers.response.LocalNetworksListHandler> },
			{ typeof(URL.my.Avatars), CreateHandler<URL.my.Avatars, handlers.response.AvatarsSettingsHandler> },
			{ typeof(URL.my.conversations.Conversation), CreateHandler<URL.my.conversations.Conversation, handlers.response.ConversationHandler> },
			{ typeof(URL.my.conversations.List), CreateHandler<URL.my.conversations.List, handlers.response.ConversationsHandler> },
			{ typeof(URL.my.conversations.NewPM), CreateHandler<URL.my.conversations.NewPM, handlers.response.PMSendHandler> },
			{ typeof(URL.my.conversations.Reply), CreateHandler<URL.my.conversations.Reply, handlers.response.PMReplyHandler> },
			{ typeof(URL.my.login.Login), CreateHandler<URL.my.login.Login, handlers.response.LoginHandler> },
			{ typeof(URL.my.login.Migrate), CreateHandler<URL.my.login.Migrate, handlers.response.MigrateAccountHandler> },
			{ typeof(URL.my.login.RegisterByInvite), CreateHandler<URL.my.login.RegisterByInvite, handlers.response.RegisterByInviteHandler> },
			{ typeof(URL.my.Settings), CreateHandler<URL.my.Settings, handlers.response.SettingsHandler> },
			{ typeof(URL.my.UserData), CreateHandler<URL.my.UserData, handlers.response.UserDataHandler> },
			{ typeof(URL.polls.Info), CreateHandler<URL.polls.Info, handlers.response.PollHandler> },
			{ typeof(URL.polls.List), CreateHandler<URL.polls.List, handlers.response.PollsListHandler> },
			{ typeof(URL.polls.New), CreateHandler<URL.polls.New, handlers.response.CreatePollHandler> },
			{ typeof(URL.QuickLink), CreateHandler<URL.QuickLink, handlers.response.QuickLinkHandler> },
			{ typeof(URL.Robots), CreateHandler<URL.Robots, handlers.response.RobotsHandler> },
			{ typeof(URL.Static), CreateHandler<URL.Static, handlers.StaticHandler> },
			{ typeof(URL.upload.Item), CreateHandler<URL.upload.Item, handlers.response.UploadHandler> },
			{ typeof(URL.upload.List), CreateHandler<URL.upload.List, handlers.response.UploadListHandler> },
			{ typeof(URL.upload.New), CreateHandler<URL.upload.New, handlers.response.UploadNewHandler> },
			{ typeof(URL.users.Active), CreateHandler<URL.users.Active, handlers.response.ActiveAccountListHandler> },
			{ typeof(URL.users.All), CreateHandler<URL.users.All, handlers.response.UserListHandler> },
			{ typeof(URL.users.Online), CreateHandler<URL.users.Online, handlers.response.WhoIsOnlineHandler> },
			{ typeof(URL.users.user.Info), CreateHandler<URL.users.user.Info, handlers.response.UserInfoHandler> },
			{ typeof(URL.users.user.PollsParticipated), CreateHandler<URL.users.user.PollsParticipated, handlers.response.UserPollsParticipatedHandler> },
			{ typeof(URL.users.user.Posts), CreateHandler<URL.users.user.Posts, handlers.response.UserPostsHandler> },
			{ typeof(URL.users.user.Replies), CreateHandler<URL.users.user.Replies, handlers.response.UserRepliesHandler> },
			{ typeof(URL.users.user.Threads), CreateHandler<URL.users.user.Threads, handlers.response.UserThreadsHandler> },
		};

		private static ISpecificHandler CreateHandler<TUrl, THandler>(AbstractUrl url)
			where TUrl : AbstractUrl
			where THandler : handlers.AbstractGetHandler<TUrl>, new()
		{
			return new THandler() {
				url = (TUrl)url
			};
		}

		public static ISpecificHandler getHandler(WebContext context) {
			if(context.httprequest.Path.ToLower().StartsWith("/do/")) {
				string action = context.httprequest.Path.ToLower().Substring(4).Trim('/');
				if(action.StartsWith("markthreadasread")) {
					return new handlers.request.MarkThreadAsReadHandler();
				}
				switch(action) {
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
					case "userdata":
						return new handlers.request.UserDataHandler();
					case "sendpm":
						return new handlers.request.SendPMHandler();
					case "upload":
						return new handlers.request.UploadHandler();
					case "newpoll":
						return new handlers.request.CreatePollHandler();
					case "vote":
						return new handlers.request.VoteHandler();
					case "avatars/add":
						return new handlers.request.avatars.AddHandler();
					case "avatars/remove":
						return new handlers.request.avatars.RemoveHandler();
					case "avatars/setasdefault":
						return new handlers.request.avatars.SetAsDefaultHandler();
					case "maintenance/cleancache":
						return new handlers.request.maintenance.CleanCacheHandler();
					default:
						return new handlers.WrongUrlHandler();
				}
			}

			AbstractUrl url = UrlManager.Parse(context.httprequest.Path, context.httprequest.QueryString, context.account != null);
			if(url == null) {
				return new handlers.WrongUrlHandler();
			}

			if(!context.httprequest.Path.StartsWith(url.canonical)) {
				//throw new ApplicationException("Going to redirect to: '" + url.canonicalFull + "' (canonical='" + url.canonicalFull + "')");
				throw new RedirectException(url.canonicalFull);
			}

			return handlersDictionary[url.GetType()](url);

		}

	}
}
