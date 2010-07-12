using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using PJonDevelopment.BBCode;
using System.IO;
using FLocal.Core;

namespace FLocal.Common {
	public static class UBBParser {

		private class BBParserGateway {

			private class TextFormatter : ITextFormatter {

				public static readonly TextFormatter instance = new TextFormatter();

				private static readonly Dictionary<string, string> SMILEYS = new Dictionary<string, string> {
					{ ":)", "smile" },
					{ ":(", "frown" },
					{ ":o", "blush" },
					{ ":D", "laugh" },
					{ ";)", "wink" },
					{ ":p", "tongue" },
					/*{ ":cool:", "cool" },
					{ ":crazy:", "crazy" },
					{ ":mad:", "mad" },
					{ ":shocked:", "shocked" },
					{ ":smirk:", "smirk" },
					{ ":grin:", "grin" },
					{ ":ooo:", "ooo" },
					{ ":confused:", "confused" },
					{ ":lol:", "lol" },*/
				};

				private static readonly Dictionary<Regex, MatchEvaluator> SMILEYS_DATA = (from smile in SMILEYS select new KeyValuePair<Regex, MatchEvaluator>(new Regex("(^|\\s|>)" + Regex.Escape(smile.Key) + "($|\\s|<)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline), match => match.Groups[1] + "<img src=\"/static/smileys/" + smile.Value + ".gif\" alt=\"" + smile.Key + "\"/>" + match.Groups[2])).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

				private static readonly Regex SMILEYS_MATCHER = new Regex("(^|\\s|>)\\:(\\w+)\\:($|\\s|<)", RegexOptions.Compiled | RegexOptions.Singleline);

				private static string SMILEYS_REPLACE(Match match) {
					FileInfo smiley = new FileInfo(Config.instance.dataDir + "Static\\smileys\\" + match.Groups[2] + ".gif");
					if(smiley.Exists && smiley.FullName.StartsWith(Config.instance.dataDir + "Static\\smileys\\")) {
						return match.Groups[1] + "<img src=\"/static/smileys/" + match.Groups[2] + ".gif\" alt=\"" + match.Groups[2] + "\"/>" + match.Groups[3];
					} else {
						return match.Value;
					}
				}

				private static readonly Regex LINKS_MATCHER = new Regex("https?://[^\\s\\[<]+", RegexOptions.Singleline | RegexOptions.Compiled);
				private static string LINKS_REPLACE(Match match) {
					return BBCodes.UrlProcessor.ProcessLink(match.Value, null, true);
				}

				private static readonly Dictionary<Regex, MatchEvaluator> TYPOGRAPHICS = new Dictionary<Regex, MatchEvaluator> {
					{ new Regex("(\\s+)--?(\\s+)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline), match => match.Groups[1] + "–" + match.Groups[2] },
					{ new Regex("(\\s+)---(\\s+)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline), match => match.Groups[1] + "—" + match.Groups[2] },
					{ new Regex("\\(c\\)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline), match => "©" },
					{ new Regex("\\(r\\)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline), match => "®" },
					{ new Regex("\\(tm\\)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline), match => "™" },
					{ new Regex("\\+\\-", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline), match => "±" },
					{ new Regex("&lt;=", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline), match => "≤" },
					{ new Regex("&gt;=", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline), match => "≥" },
					{ new Regex("!=", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline), match => "≠" },
					{ new Regex("~=", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline), match => "≈" },
				};

				private ITextFormatter inner;

				private TextFormatter() {
					this.inner = new BBCodeHtmlFormatter();
				}

				public string Format(string source) {
					string result = this.inner.Format(source).Replace("&nbsp;", " ");
					result = LINKS_MATCHER.Replace(result, LINKS_REPLACE);
					foreach(var smile in SMILEYS_DATA) {
						result = smile.Key.Replace(result, smile.Value);
					}
					result = SMILEYS_MATCHER.Replace(result, SMILEYS_REPLACE);
					foreach(var kvp in TYPOGRAPHICS) {
						result = kvp.Key.Replace(result, kvp.Value);
					}
					return result;
				}

			}

			public static readonly BBParserGateway instance = new BBParserGateway();

			private BBCodeParser parser;
			private ITextFormatter formatter;

			private BBParserGateway() {
				this.parser = new BBCodeParser();
				this.parser.ElementTypes.Add("b", typeof(BBCodes.B), true);
				this.parser.ElementTypes.Add("code", typeof(BBCodes.Code), true);
				this.parser.ElementTypes.Add("font", typeof(BBCodes.Font), true);
				this.parser.ElementTypes.Add("color", typeof(BBCodes.FontColor), true);
				this.parser.ElementTypes.Add("size", typeof(BBCodes.FontSize), true);
				this.parser.ElementTypes.Add("furl", typeof(BBCodes.FUrl), true);
				this.parser.ElementTypes.Add("google", typeof(BBCodes.Google), true);
				this.parser.ElementTypes.Add("i", typeof(BBCodes.I), true);
				this.parser.ElementTypes.Add("image", typeof(BBCodes.Image), true);
				this.parser.ElementTypes.Add("list", typeof(BBCodes.List), true);
				this.parser.ElementTypes.Add("lurk", typeof(BBCodes.Lurk), true);
				this.parser.ElementTypes.Add("*", typeof(BBCodes.ListElem), false);
				this.parser.ElementTypes.Add("poll", typeof(BBCodes.Poll), true);
				this.parser.ElementTypes.Add("quote", typeof(BBCodes.Quote), true);this.parser.ElementTypes.Add("q", typeof(BBCodes.Quote), true);
				this.parser.ElementTypes.Add("s", typeof(BBCodes.S), true);
				this.parser.ElementTypes.Add("spoiler", typeof(BBCodes.Spoiler), true);this.parser.ElementTypes.Add("cut", typeof(BBCodes.Spoiler), true);
				this.parser.ElementTypes.Add("u", typeof(BBCodes.U), true);
				this.parser.ElementTypes.Add("uploadimage", typeof(BBCodes.UploadImage), true);
				this.parser.ElementTypes.Add("uploadlink", typeof(BBCodes.UploadLink), true);
				this.parser.ElementTypes.Add("url", typeof(BBCodes.Url), true);
				this.parser.ElementTypes.Add("user", typeof(BBCodes.User), false);
				this.parser.ElementTypes.Add("wiki", typeof(BBCodes.Wiki), true);
				this.parser.ElementTypes.Add("ruwiki", typeof(BBCodes.RuWiki), true);
				this.formatter = TextFormatter.instance;
			}

			public string Parse(string input) {
				string result = this.parser.Parse(input).Format(this.formatter);
				if(result.EndsWith("<br/>")) result = result.Substring(0, result.Length - 5);
				return result;
			}

		}

		public static string UBBToIntermediate(string UBB) {
			//return HttpUtility.HtmlEncode(UBB).Replace("\r\n", "<br/>\r\n");
			return BBParserGateway.instance.Parse(UBB);
		}

		public static string ShallerToUBB(string shaller) {
			return shaller;
		}

	}
}
