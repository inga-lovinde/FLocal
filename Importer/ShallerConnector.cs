using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.Configuration;
using System.IO;

namespace FLocal.Importer {
	class ShallerConnector {

		public static Encoding encoding {
			get {
				return Encoding.GetEncoding(1251);
			}
		}

		public static string getPageContent(string requestUrl, Dictionary<string, string> postData, CookieContainer cookies) {
			string baseUrl = ConfigurationManager.AppSettings["Importer_BaseUrl"];
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl + requestUrl);
			request.KeepAlive = true;
			request.CookieContainer = cookies;
			if(postData.Count < 1) {
				request.Method = "GET";
			} else {

				StringBuilder postBuilder = new StringBuilder();
				foreach(KeyValuePair<string, string> kvp in postData) {
					postBuilder.Append(HttpUtility.UrlEncode(kvp.Key, encoding));
					postBuilder.Append('=');
					postBuilder.Append(HttpUtility.UrlEncode(kvp.Value, encoding));
				}

				byte[] postBytes = Encoding.ASCII.GetBytes(postBuilder.ToString());

				request.Method = "POST";
				request.ContentType = "application/x-www-form-urlencoded";
				request.ContentLength = postBytes.Length;

				Stream stream = request.GetRequestStream();
				stream.Write(postBytes, 0, postBytes.Length);
				stream.Close();
			}
			request.UserAgent = "ShallerConnector v0.1";
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			cookies.Add(response.Cookies);
			using(StreamReader reader = new StreamReader(response.GetResponseStream(), encoding)) {
				return reader.ReadToEnd();
			}
		}

	}
}
