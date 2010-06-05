using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core {

	static class StringExtension {

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

    }
}
