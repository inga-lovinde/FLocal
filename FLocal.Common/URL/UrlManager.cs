using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace FLocal.Common.URL {
	public static class UrlManager {

		private static string GetRemainder(string[] requestParts, int start) {
			return string.Join("/", (from i in Enumerable.Range(start, requestParts.Length - start) select requestParts[i]).ToArray());
		}

		public static AbstractUrl Parse(string Path, NameValueCollection Query, bool isLoggedIn) {

			string[] requestParts = Path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

			if(requestParts.Length < 1) {
				return new forum.Boards(GetRemainder(requestParts, 0));
			}

			#region legacy
			if(Path.ToLower().StartsWith("/user/upload/")) {
				return new upload.Item(requestParts[2], GetRemainder(requestParts, 3));
			}
			if(Path.EndsWith(".php")) {
				string[] scriptParts = Path.Split('.');
				if(scriptParts.Length != 2) {
					return null;
				}

				switch(scriptParts[0].TrimStart('/').ToLower()) {
					case "showflat":
					case "ashowflat":
						dataobjects.Post post = dataobjects.Post.LoadById(int.Parse(Query["Number"]));
						return new forum.board.thread.Posts(post.threadId.ToString(), "p" + post.id.ToString());
					case "showthreaded":
					case "ashowthreaded":
						return new forum.board.thread.post.Show(Query["Number"], "");
					case "postlist":
						return new forum.board.Threads(dataobjects.Board.LoadByLegacyName(Query["Board"]).id.ToString(), "");
					case "showprofile":
						return new users.user.Info(Query["User"], "");
					default:
						return null;
				}
			}
			if(Path.ToLower().StartsWith("/images/graemlins/") || Path.ToLower().StartsWith("/images/icons/")) {
				return new Static("smileys/" + GetRemainder(requestParts, 2));
			} else if(Path.ToLower().StartsWith("/smiles/")) {
				return new Static("smileys/" + GetRemainder(requestParts, 1));
			}
			#endregion

			#region robots
			if(Path.ToLower().StartsWith("/robots.txt")) {
				return new Robots();
			}
			#endregion

			switch(requestParts[0].ToLower()) {
				#region newlegacy
				case "allposts":
					return new forum.AllPosts(GetRemainder(requestParts, 1));
				case "allthreads":
					return new forum.AllThreads(GetRemainder(requestParts, 1));
				case "boards":
					return new forum.Boards(GetRemainder(requestParts, 1));
				case "board":
					if(requestParts.Length < 2) {
						return null;
					}
					if(requestParts.Length == 2) {
						return new forum.board.Threads(requestParts[1], "");
					}
					switch(requestParts[2].ToLower()) {
						case "threads":
							return new forum.board.Threads(requestParts[1], GetRemainder(requestParts, 3));
						case "headlines":
							return new forum.board.Headlines(requestParts[1], GetRemainder(requestParts, 3));
						case "newthread":
							return new forum.board.NewThread(requestParts[1], GetRemainder(requestParts, 3));
						default:
							return new forum.board.Threads(requestParts[1], GetRemainder(requestParts, 2));
					}
				case "boardasthread":
					return new forum.board.Headlines(requestParts[1], GetRemainder(requestParts, 2));
				case "thread":
					return new forum.board.thread.Posts(requestParts[1], GetRemainder(requestParts, 2));
				case "post":
					if(requestParts.Length < 2) {
						return null;
					}
					if(requestParts.Length == 2) {
						return new forum.board.thread.post.Show(requestParts[1], "");
					}
					switch(requestParts[2].ToLower()) {
						case "edit":
							return new forum.board.thread.post.Edit(requestParts[1], "");
						case "reply":
							return new forum.board.thread.post.Reply(requestParts[1], "");
						case "pmreply":
							return new forum.board.thread.post.PMReply(requestParts[1], "");
						case "punish":
							return new forum.board.thread.post.Punish(requestParts[1], "");
						default:
							return null;
					}
				case "registerbyinvite":
					return new my.login.RegisterByInvite(GetRemainder(requestParts, 1));
				case "user":
					return new users.user.Info(requestParts[1], GetRemainder(requestParts, 2));
				#endregion newlegacy
				#region forum
				case "forum":
					if(requestParts.Length == 1) {
						return new forum.Boards("");
					}
					switch(requestParts[1].ToLower()) {
						case "board":
							if(requestParts.Length == 2) {
								return null;
							}
							if(requestParts.Length == 3) {
								return new forum.board.Threads(requestParts[2], "");
							}
							switch(requestParts[3].ToLower()) {
								case "headlines":
									return new forum.board.Headlines(requestParts[2], GetRemainder(requestParts, 4));
								case "newthread":
									return new forum.board.NewThread(requestParts[2], GetRemainder(requestParts, 4));
								case "thread":
									if(requestParts.Length == 4) {
										return null;
									}
									if(requestParts.Length == 5) {
										return new forum.board.thread.Posts(requestParts[4], "");
									}
									switch(requestParts[5].ToLower()) {
										case "post":
											if(requestParts.Length == 6) {
												return null;
											}
											if(requestParts.Length == 7) {
												return new forum.board.thread.post.Show(requestParts[6], "");
											}
											switch(requestParts[7].ToLower()) {
												case "edit":
													return new forum.board.thread.post.Edit(requestParts[6], GetRemainder(requestParts, 8));
												case "pmreply":
													return new forum.board.thread.post.PMReply(requestParts[6], GetRemainder(requestParts, 8));
												case "punish":
													return new forum.board.thread.post.Punish(requestParts[6], GetRemainder(requestParts, 8));
												case "reply":
													return new forum.board.thread.post.Reply(requestParts[6], GetRemainder(requestParts, 8));
												case "show":
													return new forum.board.thread.post.Show(requestParts[6], GetRemainder(requestParts, 8));
												default:
													return null;
											}
										case "posts":
											return new forum.board.thread.Posts(requestParts[4], GetRemainder(requestParts, 6));
										default:
											return null;
									}
								case "threads":
									return new forum.board.Threads(requestParts[2], GetRemainder(requestParts, 4));
								default:
									return null;
							}
						case "allposts":
							return new forum.AllPosts(GetRemainder(requestParts, 2));
						case "allthreads":
							return new forum.AllThreads(GetRemainder(requestParts, 2));
						case "boards":
							return new forum.Boards(GetRemainder(requestParts, 2));
						default:
							return null;
					}
				#endregion forum
				#region maintenance
				case "maintenance":
					if(requestParts.Length < 2) {
						return null;
					}
					switch(requestParts[1].ToLower()) {
						case "cleancache":
							return new maintenance.CleanCache(GetRemainder(requestParts, 2));
						case "localnetworks":
							return new maintenance.LocalNetworks(GetRemainder(requestParts, 1));
						default:
							return null;
					}
				#endregion maintenance
				#region my;
				case "my":
					if(requestParts.Length == 1) {
						if(isLoggedIn) {
							return new my.conversations.List("");
						} else {
							return new my.login.Login("");
						}
					}
					switch(requestParts[1].ToLower()) {
						case "conversations":
							if(requestParts.Length == 2) {
								return new my.conversations.List("");
							}
							switch(requestParts[2].ToLower()) {
								case "conversation":
									return new my.conversations.Conversation(requestParts[3], GetRemainder(requestParts, 4));
								case "list":
									return new my.conversations.List(GetRemainder(requestParts, 3));
								case "newpm":
								case "pmsend":
									return new my.conversations.NewPM(GetRemainder(requestParts, 3));
								case "reply":
								case "pmreply":
									return new my.conversations.Reply(requestParts[3], GetRemainder(requestParts, 4));
								default:
									return null;
							}
						case "login":
							if(requestParts.Length == 2) {
								return new my.login.Login("");
							}
							switch(requestParts[2].ToLower()) {
								case "login":
									return new my.login.Login(GetRemainder(requestParts, 3));
								case "migrate":
								case "migrateaccount":
									return new my.login.Migrate(GetRemainder(requestParts, 3));
								case "registerbyinvite":
									return new my.login.RegisterByInvite(GetRemainder(requestParts, 3));
								default:
									return null;
							}
						case "avatars":
							return new my.Avatars(GetRemainder(requestParts, 2));
						case "settings":
							return new my.Settings(GetRemainder(requestParts, 2));
						case "userdata":
							return new my.UserData(GetRemainder(requestParts, 2));
						default:
							return null;
					}
				#endregion my;
				#region polls
				case "poll":
					if(requestParts.Length < 2) {
						return null;
					}
					return new polls.Info(requestParts[1], GetRemainder(requestParts, 2));
				case "polls":
					if(requestParts.Length < 2) {
						return new polls.List("");
					}
					switch(requestParts[1].ToLower()) {
						case "info":
							return new polls.Info(requestParts[2], GetRemainder(requestParts, 3));
						case "new":
							return new polls.New(GetRemainder(requestParts, 2));
						case "list":
							return new polls.List(GetRemainder(requestParts, 2));
						default:
							return null;
					}
				#endregion polls
				#region upload;
				case "upload":
					if(requestParts.Length < 2) {
						return new upload.List("");
					}
					switch(requestParts[1].ToLower()) {
						case "item":
							return new upload.Item(requestParts[2], GetRemainder(requestParts, 3));
						case "info":
							return new upload.Info(requestParts[2], GetRemainder(requestParts, 3));
						case "list":
							return new upload.List(GetRemainder(requestParts, 2));
						case "new":
							return new upload.New(GetRemainder(requestParts, 2));
						default:
							return null;
					}
				#endregion upload;
				#region users;
				case "users":
					if(requestParts.Length == 1) {
						return new users.All("");
					}
					switch(requestParts[1].ToLower()) {
						case "user":
							if(requestParts.Length < 3) {
								return null;
							}
							if(requestParts.Length == 3) {
								return new users.user.Info(requestParts[2], "");
							}
							switch(requestParts[3].ToLower()) {
								case "info":
									return new users.user.Info(requestParts[2], GetRemainder(requestParts, 4));
								case "pollsparticipated":
									return new users.user.PollsParticipated(requestParts[2], GetRemainder(requestParts, 4));
								case "posts":
									return new users.user.Posts(requestParts[2], GetRemainder(requestParts, 4));
								case "replies":
								case "mentions":
									return new users.user.Mentions(requestParts[2], GetRemainder(requestParts, 4));
								case "threads":
									return new users.user.Threads(requestParts[2], GetRemainder(requestParts, 4));
								default:
									return null;
							}
						case "active":
							return new users.Active(GetRemainder(requestParts, 2));
						case "all":
							return new users.All(GetRemainder(requestParts, 2));
						case "online":
							return new users.Online(GetRemainder(requestParts, 2));
						default:
							return null;
					}
				#endregion users;
				case "q":
					return new QuickLink(requestParts[1], GetRemainder(requestParts, 2));
				case "robots.txt":
					return new Robots();
				case "static":
					return new Static(GetRemainder(requestParts, 1));
				default:
					return null;
			}
		}

		public static string TryGetTitle(string Path) {
			try {
				AbstractUrl url = Parse(Path, new NameValueCollection(), true);
				return url.title;
			} catch(Exception) {
				return Path;
			}
		}

	}
}
