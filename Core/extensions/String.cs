using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Core {

	public static class StringExtension {

        public static string PHPSubstring(this string str, int start, int length) {
            if(start > str.Length) {
                return "";
            } else if(start + length > str.Length) {
                return str.Substring(start);
            } else {
                return str.Substring(start, length);
            }
        }

        public static string PHPSubstring(this string str, int start) {
            if(start > str.Length) {
                return "";
            } else {
                return str.Substring(start);
            }
        }

        public static string Enquote(this string str, string quotationMark) {
            return quotationMark + str + quotationMark;
        }

        public static string Enquote(this string str) {
            return str.Enquote("'");
        }

        public static string Replace(this string src, IEnumerable<KeyValuePair<string, string>> replacePairs) {
            string dest = src;
            foreach(KeyValuePair<string, string> kvp in replacePairs) {
                dest = dest.Replace(kvp.Key, kvp.Value);
            }
            return dest;
        }

        public static bool Contains(this string str, IEnumerable<string> needles) {
            return (from needle in needles select str.Contains(needle)).Contains(true);
        }

        public static bool IContains(this string str, IEnumerable<string> needles) {
            return str.ToLower().Contains(from needle in needles select needle.ToLower());
        }

		private static readonly string[] TrimHtml_EmptyFragments = new string[] {
			"<br/>",
			"<br />",
			"&nbsp;",
		};
		public static string TrimHtml(this string str) {
			string result = str.Trim();
			foreach(var fragment in TrimHtml_EmptyFragments) {
				if(result.StartsWith(fragment)) {
					return result.Substring(fragment.Length).TrimHtml();
				}
				if(result.EndsWith(fragment)) {
					return result.Substring(0, result.Length-fragment.Length).TrimHtml();
				}
			}
			return result;
		}

		public static string Join<T>(this IEnumerable<T> parts, string separator) {
			return String.Join(separator, (from part in parts select part.ToString()).ToArray());
		}

    }
}
