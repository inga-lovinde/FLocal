using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FLocal.Importer {
	public static class DictionaryConverter {

		public static string ToDump(Dictionary<string, string> dict) {
			return string.Join(" ", (from kvp in dict select HttpUtility.UrlEncode(kvp.Key, ShallerConnector.encoding) + "=" + HttpUtility.UrlEncode(kvp.Value, ShallerConnector.encoding)).ToArray());
		}

		public static Dictionary<string, string> FromDump(string dump) {
			return (from elem in dump.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) let parts = elem.Split(new char[] { '=' }, 2) select new KeyValuePair<string, string>(HttpUtility.UrlDecode(parts[0], ShallerConnector.encoding), HttpUtility.UrlDecode(parts[1], ShallerConnector.encoding))).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
		}
	
	}
}
