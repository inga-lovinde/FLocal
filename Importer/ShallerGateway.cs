using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace FLocal.Importer {
	public class ShallerGateway {

		public static string getUserInfoAsString(string userName) {
			return ShallerConnector.getPageContent("showprofile.php?User=" + userName + "&What=login&showlite=l", new Dictionary<string,string>(), new System.Net.CookieContainer());
		}

		private static Dictionary<string, Regex> regexInfoCache = new Dictionary<string, Regex>();
		private static Regex getInfoRegexByCaption(string caption) {
			if(!regexInfoCache.ContainsKey(caption)) {
				lock(caption) {
					if(!regexInfoCache.ContainsKey(caption)) {
						regexInfoCache[caption] = new Regex("<td[^>]*>\\s*" + caption + "\\s*</td>\\s*<td>\\s*([^<>]*)\\s*</td>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
					}
				}
			}
			return regexInfoCache[caption];
		}

		private static Dictionary<string, string> userImportStructure {
			get {
				return new Dictionary<string,string>() {
					{ "regDate", "Дата\\s+регистрации" },
					{ "signature", "Подпись" },
					{ "title", "Титул" },
					{ "location", "Расположение" },
					{ "biography", "Биография" },
				};
			}
		}

		public static Dictionary<string, string> getUserInfo(string userName) {
			string content = getUserInfoAsString(userName);
			return userImportStructure.ToDictionary<KeyValuePair<string, string>, string, string>(
				kvp => kvp.Key,
				kvp => HttpUtility.HtmlDecode(getInfoRegexByCaption(kvp.Value).Match(content).Groups[1].Value).Trim()
			);
		}

		public static IEnumerable<string> getUserNames(int pageNum) {
			string content = ShallerConnector.getPageContent("showmembers.php?Cat=&sb=13&page=" + pageNum + "&showlite=l", new Dictionary<string,string>(), new System.Net.CookieContainer());
			Regex matcher = new Regex(";User=([^&]+)&", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);
			MatchCollection matches = matcher.Matches(content);
			HashSet<string> result = new HashSet<string>();
			foreach(Match match in matches) {
				result.Add(match.Groups[1].Value);
			}
			return result;
		}

	}
}
