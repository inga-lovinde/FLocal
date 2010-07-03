using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using FLocal.Core;
using FLocal.Importer;
using FLocal.Common.actions;
using FLocal.Common.dataobjects;

namespace FLocal.ImportConsole {
	static class ShallerDBProcessor {

		private readonly static Dictionary<int, string> discussions = new Dictionary<int, string> {
			{ 384486, "Common.Photos" },
		};

		private readonly static DateTime UNIX = new DateTime(1970, 1, 1, 0, 0, 0);

		public static void processDB(string filename) {
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
						bool exists = false;
						try {
							Post post = Post.LoadById(postId);
							exists = true;
						} catch(NotFoundInDBException) {
						}
						if(exists) {
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
								Console.WriteLine("Cannot find user " + username);
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
					}
				}
			}
		}

	}
}
