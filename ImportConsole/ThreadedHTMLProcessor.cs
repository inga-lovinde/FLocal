using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace FLocal.Migration.Console {
	class ThreadedHTMLProcessor {

		private readonly static DateTime UNIX = new DateTime(1970, 1, 1, 0, 0, 0);

		private readonly static Regex PARENT_BEGINMARKER = new Regex("<font class=\"small\">\\[<a href=\"/a?showthreaded.php\\?Cat=&amp;Board=\\w+&amp;Number=");
		private const string PARENT_ENDMARKER = "&amp;";
		private readonly static Regex DATE_MATCH = new Regex("(\\d\\d)\\.(\\d\\d)\\.(\\d\\d\\d\\d)\\s*(\\d\\d):(\\d\\d)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
		private const string POSTER_BEGINMARKER = "<a href=\"/showprofile.php?Cat=&amp;User=";
		private const string POSTER_BEGINMARKER_GUEST = "<a href=\"/showip.php?Cat=&amp;IP=";
		private const string POSTER_ENDMARKER = "&amp;";
		private const string BOARD_BEGINMARKER = "/postlist.php?Cat=&amp;Board=";
		private const string BOARD_ENDMARKER = "&amp;";

		private const string POST_BEGINMARKER_FULL = "<font class=\"post\">";
		private const string POST_ENDMARKER_FULL_SIGNATURE = "<div style=\"width:100%;max-height:50px;overflow:hidden\">";
		private readonly static Regex POST_ENDMARKER_FULL = new Regex("</font>\\s*</td>\\s*</tr>\\s*</table>\\s*</td>\\s*</tr>\\s*</table>\\s*<br />\\s*<table\\s*width=\"95%\"\\s*align=\"center\"\\s*cellpadding=\"1\"\\s*cellspacing=\"1\"\\s*class=\"tablesurround\">", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
		private const string POST_ENDMARKER_FULL_DISCUSSION = "<br><br><font class=\"small\"><a href=\"/newreply.php?";
		private readonly static Regex TITLE_BEGINMARKER_FULL = new Regex("<td width=\"83%\" class=\"subjecttable\">\\s*<table width=\"100%\" class=\"subjecttable\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\\s*<tr>\\s*<td align=\"left\" width=\"70%\">.*<b>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
		private const string TITLE_ENDMARKER_FULL = "</b>";
		private const string THREAD_BEGINMARKER_FULL = "type=favorite&amp;Number=";
		private const string THREAD_ENDMARKER_FULL = "&amp";

		private const string POST_BEGINMARKER_LITE = "<hr>";
		private const string POST_ENDMARKER_LITE = "</body>";
		private const string TITLE_BEGINMARKER_LITE = "<b>";
		private const string TITLE_ENDMARKER_LITE = "</b>";
		private const string THREAD_BEGINMARKER_LITE = "view=&amp;sb=&amp;o=&amp;thread=";
		private const string THREAD_ENDMARKER_LITE = "&amp";


		public static void Process(string pathToThreadeds, string pathToOutput) {

			using(StreamWriter writer = new StreamWriter(pathToOutput, false, Encoding.ASCII)) {
				DirectoryInfo directoryInfo = new DirectoryInfo(pathToThreadeds);
				int i=0;
				foreach(FileSystemInfo _info in directoryInfo.GetFiles()) {
					if(i%100 == 0) {
						System.Console.Write("[" + (int)(i/100) + "]");
					}
					if(!(_info is FileInfo)) continue;
					FileInfo info = (FileInfo)_info;
					string[] parts = info.Name.Split('.');
					if((parts.Length != 2) || (parts[1].ToLower() != "txt")) continue;
					int postId = int.Parse(parts[0]);
					try {
						string contentPost;
						string contentTitle;
						DateTime contentDate;
						int? contentParent = null;
						int contentThread;
						string contentPoster;
						int contentLayerId = 1;
						string contentBoard;
						using(StreamReader reader = new StreamReader(info.FullName, FLocal.Migration.Gateway.ShallerGateway.encoding)) {
							string raw = reader.ReadToEnd();
							if(raw.Contains("-CATJUMP-1")) {
								//full mode
								string beforeBegin;
								{
									int beginPos = raw.IndexOf(POST_BEGINMARKER_FULL);
									if(beginPos <= 0) throw new ApplicationException("beginPos <= 0");
									beforeBegin = raw.Substring(0, beginPos);
									string afterBegin = raw.Substring(beginPos + POST_BEGINMARKER_FULL.Length);
									int endPos;
									endPos = afterBegin.IndexOf(POST_ENDMARKER_FULL_DISCUSSION);
									if(endPos <= 0) {
										endPos = afterBegin.IndexOf(POST_ENDMARKER_FULL_SIGNATURE);
										if(endPos <= 0) {
											Match endBodyMatch = POST_ENDMARKER_FULL.Match(afterBegin);
											if(!endBodyMatch.Success) {
												System.Console.WriteLine("afterBegin:");
												System.Console.WriteLine("===========================");
												System.Console.WriteLine(afterBegin);
												System.Console.WriteLine("===========================");
												System.Console.WriteLine(POST_ENDMARKER_FULL.ToString());
												throw new ApplicationException("cannot match body end");
											}
											endPos = endBodyMatch.Index;
										}
									}
									contentPost = afterBegin.Substring(0, endPos);
								}

								{
									Match titleMatch = TITLE_BEGINMARKER_FULL.Match(beforeBegin);
									if(!titleMatch.Success) throw new ApplicationException("cannot match title begin");
									string afterTitleBegin = beforeBegin.Substring(titleMatch.Index + titleMatch.Length);
									int titleEndPos = afterTitleBegin.IndexOf(TITLE_ENDMARKER_FULL);
									if(titleEndPos <= 0) {
										throw new ApplicationException("titleEndPos <= 0");
									}
									contentTitle = afterTitleBegin.Substring(0, titleEndPos);
								}

								{
									Match dateMatch = DATE_MATCH.Match(beforeBegin);
									if(!dateMatch.Success) {
										throw new ApplicationException("cannot match date");
									}
									contentDate = new DateTime(int.Parse(dateMatch.Groups[3].Value), int.Parse(dateMatch.Groups[2].Value), int.Parse(dateMatch.Groups[1].Value), int.Parse(dateMatch.Groups[4].Value), int.Parse(dateMatch.Groups[5].Value), 0);
								}

								{
									Match parentMatch = PARENT_BEGINMARKER.Match(beforeBegin);
									if(parentMatch.Success) {
										string afterParentBegin = beforeBegin.Substring(parentMatch.Index + parentMatch.Length);
										int parentEndPos = afterParentBegin.IndexOf(PARENT_ENDMARKER);
										if(parentEndPos <= 0) {
											throw new ApplicationException("parentEndPos <= 0");
										}
										contentParent = int.Parse(afterParentBegin.Substring(0, parentEndPos));
									}
								}

								{
									int posterBeginPos = beforeBegin.IndexOf(POSTER_BEGINMARKER);
									if(posterBeginPos > 0) {
										string afterPosterBegin = beforeBegin.Substring(posterBeginPos + POSTER_BEGINMARKER.Length);
										int posterEndPos = afterPosterBegin.IndexOf(POSTER_ENDMARKER);
										if(posterEndPos <= 0) {
											throw new ApplicationException("posterEndPos <= 0");
										}
										contentPoster = afterPosterBegin.Substring(0, posterEndPos);
									} else {
										posterBeginPos = beforeBegin.IndexOf(POSTER_BEGINMARKER_GUEST);
										if(posterBeginPos <= 0) {
											throw new ApplicationException("posterBeginPos <= 0");
										}
										string afterPosterBegin = beforeBegin.Substring(posterBeginPos + POSTER_BEGINMARKER_GUEST.Length);
										int posterEndPos = afterPosterBegin.IndexOf(POSTER_ENDMARKER);
										if(posterEndPos <= 0) {
											throw new ApplicationException("posterEndPos <= 0");
										}
										contentPoster = "Guest " + afterPosterBegin.Substring(0, posterEndPos);
									}
								}

								{
									int threadBeginPos = raw.IndexOf(THREAD_BEGINMARKER_FULL);
									if(threadBeginPos <= 0) {
										throw new ApplicationException("threadbeginpos <= 0");
									}
									string afterThreadBegin = raw.Substring(threadBeginPos + THREAD_BEGINMARKER_FULL.Length);
									int threadEndPos = afterThreadBegin.IndexOf(THREAD_ENDMARKER_FULL);
									if(threadEndPos <= 0) {
										throw new ApplicationException("threadEndPos <= 0");
									}
									contentThread = int.Parse(afterThreadBegin.Substring(0, threadEndPos));
								}

								{
									int boardBeginPos = beforeBegin.IndexOf(BOARD_BEGINMARKER);
									if(boardBeginPos <= 0) {
										throw new ApplicationException("boardbeginpos <= 0");
									}
									string afterBoardBegin = beforeBegin.Substring(boardBeginPos + BOARD_BEGINMARKER.Length);
									int boardEndPos = afterBoardBegin.IndexOf(BOARD_ENDMARKER);
									if(boardEndPos <= 0) {
										throw new ApplicationException("boardEndPos <= 0");
									}
									contentBoard = afterBoardBegin.Substring(0, boardEndPos);
								}

								if(beforeBegin.IndexOf("trash.gif") > 0) {
									contentLayerId = 3;
								} else if(beforeBegin.IndexOf("eye.gif") > 0) {
									contentLayerId = 2;
								}

							} else {
								//lite mode
								string beforeBegin;
								{
									int beginPos = raw.IndexOf(POST_BEGINMARKER_LITE);
									if(beginPos <= 0) throw new ApplicationException("beginPos <= 0");
									beforeBegin = raw.Substring(0, beginPos);
									string afterBegin = raw.Substring(beginPos + POST_BEGINMARKER_LITE.Length);
									int endPos;
									endPos = afterBegin.IndexOf(POST_ENDMARKER_LITE);
									if(endPos <= 0) {
										throw new ApplicationException("cannot match body end");
									}
									contentPost = afterBegin.Substring(0, endPos);
								}

								{
									int titleBeginPos = beforeBegin.IndexOf(TITLE_BEGINMARKER_LITE);
									if(titleBeginPos <= 0) {
										throw new ApplicationException("titlebeginpos <= 0");
									}
									string afterTitleBegin = beforeBegin.Substring(titleBeginPos + TITLE_BEGINMARKER_LITE.Length);
									int titleEndPos = afterTitleBegin.IndexOf(TITLE_ENDMARKER_LITE);
									if(titleEndPos <= 0) {
										throw new ApplicationException("titleEndPos <= 0");
									}
									contentTitle = afterTitleBegin.Substring(0, titleEndPos);
								}

								{
									Match dateMatch = DATE_MATCH.Match(beforeBegin);
									if(!dateMatch.Success) {
										throw new ApplicationException("cannot match date");
									}
									contentDate = new DateTime(int.Parse(dateMatch.Groups[3].Value), int.Parse(dateMatch.Groups[2].Value), int.Parse(dateMatch.Groups[1].Value), int.Parse(dateMatch.Groups[4].Value), int.Parse(dateMatch.Groups[5].Value), 0);
								}

								{
									Match parentMatch = PARENT_BEGINMARKER.Match(beforeBegin);
									if(parentMatch.Success) {
										string afterParentBegin = beforeBegin.Substring(parentMatch.Index + parentMatch.Length);
										int parentEndPos = afterParentBegin.IndexOf(PARENT_ENDMARKER);
										if(parentEndPos <= 0) {
											throw new ApplicationException("parentEndPos <= 0");
										}
										contentParent = int.Parse(afterParentBegin.Substring(0, parentEndPos));
									}
								}

								{
									int posterBeginPos = beforeBegin.IndexOf(POSTER_BEGINMARKER);
									if(posterBeginPos > 0) {
										string afterPosterBegin = beforeBegin.Substring(posterBeginPos + POSTER_BEGINMARKER.Length);
										int posterEndPos = afterPosterBegin.IndexOf(POSTER_ENDMARKER);
										if(posterEndPos <= 0) {
											throw new ApplicationException("posterEndPos <= 0");
										}
										contentPoster = afterPosterBegin.Substring(0, posterEndPos);
									} else {
										posterBeginPos = beforeBegin.IndexOf(POSTER_BEGINMARKER_GUEST);
										if(posterBeginPos <= 0) {
											//if(!beforeBegin.Contains("Anonymous")) {
												//throw new ApplicationException("posterBeginPos <= 0");
											//} else {
												contentPoster = "Anonymous";
											//}
										} else {
											string afterPosterBegin = beforeBegin.Substring(posterBeginPos + POSTER_BEGINMARKER_GUEST.Length);
											int posterEndPos = afterPosterBegin.IndexOf(POSTER_ENDMARKER);
											if(posterEndPos <= 0) {
												throw new ApplicationException("posterEndPos <= 0");
											}
											contentPoster = "Guest " + afterPosterBegin.Substring(0, posterEndPos);
										}
									}
								}

								{
									int threadBeginPos = beforeBegin.IndexOf(THREAD_BEGINMARKER_LITE);
									if(threadBeginPos <= 0) {
										throw new ApplicationException("threadbeginpos <= 0");
									}
									string afterThreadBegin = beforeBegin.Substring(threadBeginPos + THREAD_BEGINMARKER_LITE.Length);
									int threadEndPos = afterThreadBegin.IndexOf(THREAD_ENDMARKER_LITE);
									if(threadEndPos <= 0) {
										throw new ApplicationException("threadEndPos <= 0");
									}
									contentThread = int.Parse(afterThreadBegin.Substring(0, threadEndPos));
								}

								{
									int boardBeginPos = beforeBegin.IndexOf(BOARD_BEGINMARKER);
									if(boardBeginPos <= 0) {
										throw new ApplicationException("boardbeginpos <= 0");
									}
									string afterBoardBegin = beforeBegin.Substring(boardBeginPos + BOARD_BEGINMARKER.Length);
									int boardEndPos = afterBoardBegin.IndexOf(BOARD_ENDMARKER);
									if(boardEndPos <= 0) {
										throw new ApplicationException("boardEndPos <= 0");
									}
									contentBoard = afterBoardBegin.Substring(0, boardEndPos);
								}

								if(beforeBegin.IndexOf("(xx)") > 0) {
									contentLayerId = 3;
								} else if(beforeBegin.IndexOf("(x)") > 0) {
									contentLayerId = 2;
								}
							}
						}
						if(!contentParent.HasValue) contentParent = 0;
						contentTitle = contentTitle.Trim();
						contentPost = contentPost.Trim();
						/*Console.WriteLine("=============================");
						Console.WriteLine("PostId: " + postId.ToString());
						Console.WriteLine("Board: " + contentBoard);
						Console.WriteLine("Layer: " + contentLayerId.ToString());
						Console.WriteLine("Date: " + contentDate.ToString());
						Console.WriteLine("Parent: " + contentParent.ToString());
						Console.WriteLine("Thread: " + contentThread.ToString());
						Console.WriteLine("Poster: " + contentPoster);
						Console.WriteLine("Title: " + contentTitle);
						Console.WriteLine("Body: " + contentPost);
						Console.ReadLine();*/
						writer.WriteLine(
							FLocal.Migration.Gateway.DictionaryConverter.ToDump(
								new Dictionary<string, string> {
									{ "Subject", contentTitle },
									{ "Board", contentBoard },
									{ "UnixTime", ((int)(contentDate.ToUniversalTime().Subtract(UNIX).TotalSeconds)).ToString() },
									{ "Parent", contentParent.ToString() },
									{ "Main", contentThread.ToString() },
									{ "Local_Main", contentThread.ToString() },
									{ "Username", contentPoster },
									{ "Body", contentPost },
									{ "Layer", contentLayerId.ToString() },
								}
							)
						);
						System.Console.Write("+");
					} catch(Exception e) {
						System.Console.Error.WriteLine("Could not process post #" + postId + ": " + e.GetType().FullName + ": " + e.Message);
						System.Console.Error.WriteLine(e.StackTrace);
					} finally {
						i++;
					}
				}
			}

		}

	}
}
