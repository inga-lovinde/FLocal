using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common {
	static class TranslitManager {

		private static readonly string SAFE_SOURCE      = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZабвгдеёжзийклмнопрстуфхцчшщьыъэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЬЫЪЭЮЯ -0123456789";
		private static readonly string SAFE_DESTINATION = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZabvgdeejziyklmnoprstufxc4ww'y'eu9ABVGDEEJZIJKLMNOPRSTUFXC4WW'Y'EU9--0123456789";
		private static readonly Dictionary<char, char> SAFE_REPLACEMENTS = Enumerable.Range(0, SAFE_SOURCE.Length).ToDictionary(i => SAFE_SOURCE[i], i => SAFE_DESTINATION[i]);

/*		private static readonly Dictionary<char, char> replacements = new Dictionary<char,char> {
			{ 'a', 'a' },
			{ 'b', 'b' },
			{ 'c', 'c' },
			{ 'd', 'd' },
			{ 'e', 'e' },
			{ 'f', 'f' },
			{ 'g', 'g' },
			{ 'h', 'h' },
			{ 'i', 'i' },
			{ 'j', 'j' },
			{ 'k', 'k' },
			{ 'l', 'l' },
			{ 'm', 'm' },
			{ 'n', 'n' },
			{ 'o', 'o' },
			{ 'p', 'p' },
			{ 'q', 'q' },
			{ 'r', 'r' },
			{ 's', 's' },
			{ 't', 't' },
			{ 'u', 'u' },
			{ 'v', 'v' },
			{ 'w', 'w' },
			{ 'x', 'x' },
			{ 'y', 'y' },
			{ 'z', 'z' },
			{ 'A', 'A' },
			{ 'B', 'B' },
			{ 'C', 'C' },
			{ 'D', 'D' },
//			{ '
		};*/

		private static string Transform(string source, Dictionary<char, char> transforms) {
			return new string((from i in Enumerable.Range(0, source.Length) where transforms.ContainsKey(source[i]) select transforms[source[i]]).ToArray());
		}

		public static string Translit(string source) {
			/*Dictionary<char, int> dict = new Dictionary<char,int>();
			(
				from i in Enumerable.Range(0, SAFE_SOURCE.Length)
				select SAFE_SOURCE[i]
			).Aggregate(0, (i, ch) => {
				if(!dict.ContainsKey(ch)) dict[ch] = 0;
				dict[ch]++;
				return i;
			});
			throw new ApplicationException("!" + new string((from kvp in dict where kvp.Value > 1 select kvp.Key).ToArray()) + "@");*/
			return Transform(source, SAFE_REPLACEMENTS);
		}

	}
}
