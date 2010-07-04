using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using FLocal.Core;
using FLocal.Importer;
using FLocal.Common;
using FLocal.Common.actions;
using FLocal.Common.dataobjects;
using FLocal.Core.DB;
using FLocal.Core.DB.conditions;

namespace FLocal.ImportConsole {
	static class ShallerDBProcessor {

		private readonly static Dictionary<int, string> discussions = new Dictionary<int, string> {
			{ 384486, "Common.Photos" },
			{ 2665162, "sport" }, //Обсуждение игроков 
			{ 2099333, "hobby" }, //Клуб загадывателей ников 
			{ 2189773, "automoto" }, //ПРедлагаю ТОПИК! ФОТОГРАФИИ АВТО ФОРУМЧАН! И АВТО МИРА! (originally from common)
			{ 1961373, "sport" }, //Результаты европейских чемпионатов
			{ 2334188, "common" }, //Some foto about shanghai.
			{ 2467452, "hobby" }, //Пентагон, ЧГК, десяточка и т.д. 

		};

		private readonly static DateTime UNIX = new DateTime(1970, 1, 1, 0, 0, 0);

		public static void processDB(string filename) {
			HashSet<int> discussionsIds = new HashSet<int>();
			using(StreamReader reader = new StreamReader(filename)) {
				int i=0;
				while(!reader.EndOfStream) {
					string line = reader.ReadLine().Trim();
					if(line == "") {
						continue;
					}
					if(i%1000 == 0) {
						Console.Write("[" + (int)(i/1000) + "]");
					}
					Dictionary<string, string> data = DictionaryConverter.FromDump(line);
					int postId = int.Parse(data["Number"]);
					try {
						if(Config.instance.mainConnection.GetCountByConditions(Post.TableSpec.instance, new ComparisonCondition(Post.TableSpec.instance.getIdSpec(), ComparisonType.EQUAL, postId.ToString())) > 0) {
							Console.Write("-");
						} else {
							int localMain = int.Parse(data["Local_Main"]);
							int main = int.Parse(data["Main"]);
							DateTime date = UNIX.AddSeconds(int.Parse(data["UnixTime"])).ToLocalTime();
							User user;
							string username = data["Username"];
							try {
								user = User.LoadByName(username);
							} catch(NotFoundInDBException) {
								Console.Error.WriteLine("Cannot find user '" + username + "'; creating one...");
								ChangeSetUtil.ApplyChanges(
									new InsertChange(
										User.TableSpec.instance,
										new Dictionary<string, AbstractFieldValue> {
											{ User.TableSpec.FIELD_NAME, new ScalarFieldValue(username) },
											{ User.TableSpec.FIELD_REGDATE, new ScalarFieldValue(date.ToUTCString()) },
											{ User.TableSpec.FIELD_SHOWPOSTSTOUSERS, new ScalarFieldValue(User.ENUM_SHOWPOSTSTOUSERS_ALL) },
											{ User.TableSpec.FIELD_BIOGRAPHY, new ScalarFieldValue("") },
											{ User.TableSpec.FIELD_LOCATION, new ScalarFieldValue("") },
											{ User.TableSpec.FIELD_SIGNATURE, new ScalarFieldValue("") },
											{ User.TableSpec.FIELD_TITLE, new ScalarFieldValue("") },
											{ User.TableSpec.FIELD_TOTALPOSTS, new ScalarFieldValue("0") },
											{ User.TableSpec.FIELD_USERGROUPID, new ScalarFieldValue("1") },
										}
									)
								);
								user = User.LoadByName(data["Username"]);
							}
							string title = data["Subject"];
							string body = data["Body"];
							PostLayer layer = PostLayer.LoadById(1);
							if(data.ContainsKey("Layer")) {
								layer = PostLayer.LoadById(int.Parse(data["Layer"]));
							}
							if(postId == main || postId == localMain) {
								//first post in the thread
								string legacyBoardName;
								if(localMain != 0) {
									discussionsIds.Add(main);
									legacyBoardName = discussions[main];
								} else {
									legacyBoardName = data["Board"];
								}
								Board board;
								try {
									board = Board.LoadByLegacyName(legacyBoardName);
								} catch(NotFoundInDBException) {
									throw new ApplicationException("Cannot find board '" + legacyBoardName + "'");
								}
								board.CreateThread(user, title, body, layer, date, postId);
							} else {
								int parentId = int.Parse(data["Parent"]);
								if(parentId == 0) {
									parentId = localMain;
									if(parentId == 0) {
										parentId = main;
									}
								}
								Post post;
								try {
									post = Post.LoadById(parentId);
								} catch(NotFoundInDBException) {
									throw new ApplicationException("Cannot find parent post #" + parentId);
								}
								post.Reply(user, title, body, layer, date, postId);
							}
							Console.Write("+");
						}
					} catch(Exception e) {
						Console.Error.WriteLine("Cannot process post #" + postId + ": " + e.GetType().FullName + ": " + e.Message);
						Console.Error.WriteLine(e.StackTrace);
						Console.Write("!");
//						Console.ReadLine();
					} finally {
						i++;
						if((i%50000)==0) {
							Core.RegistryCleaner.CleanRegistry<int, Post>();
							Core.RegistryCleaner.CleanRegistry<int, Thread>();
							GC.Collect();
							Console.Error.WriteLine();
							Console.Error.WriteLine("Registry cleaned; garbage collected");
							Console.Error.WriteLine();
						}
					}
				}
			}

			Console.WriteLine("Not found discussions:");
			foreach(int discussionId in discussionsIds.OrderBy(id => id)) {
				Console.WriteLine(discussionId);
			}
		}

	}
}
