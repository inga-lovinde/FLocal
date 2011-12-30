using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.Configuration;
using System.IO;

namespace FLocal.Migration.Gateway {
	class ShallerConnector {

		public static readonly Encoding encoding = Encoding.GetEncoding(1251);
		private const int BUFFER = 1024;

		public static FileInfo getPageInfo(string requestUrl, Dictionary<string, string> postData, CookieContainer cookies) {
			string baseUrl = ConfigurationManager.AppSettings["Importer_BaseUrl"];
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl + requestUrl);
			request.KeepAlive = true;
			request.CookieContainer = cookies;
			request.ReadWriteTimeout = 3*1000;
			request.Timeout = 3*1000;
			request.Accept = "*";
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
			request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; FDM; .NET4.0C; .NET4.0E)";
			using(HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
				cookies.Add(response.Cookies);

				byte[] content;
				using(Stream responseStream = response.GetResponseStream()) {
					using(MemoryStream memoryStream = new MemoryStream()) {
						byte[] buffer = new byte[BUFFER];
						int bytes;
						while((bytes = responseStream.Read(buffer, 0, BUFFER)) > 0) {
							memoryStream.Write(buffer, 0, bytes);
						}
						content = memoryStream.ToArray();
					}
				}
				if(response.ContentLength > 0 && content.Length != response.ContentLength) {
					throw new ApplicationException("incomplete file (expected " + response.ContentLength + ", got " + content.Length + ")");
				}

				FileInfo result = new FileInfo {
					dataStream = new MemoryStream(content),
					fileName = response.ResponseUri.Segments.Last(),
					filePath = response.ResponseUri.AbsolutePath,
					fileSize = response.ContentLength,
					lastModified = response.LastModified,
				};
				response.Close();
				return result;
			}
		}

		public static string getPageContent(string requestUrl, Dictionary<string, string> postData, CookieContainer cookies) {
			FileInfo info = getPageInfo(requestUrl, postData, cookies);
			using(StreamReader reader = new StreamReader(info.dataStream, encoding)) {
				return reader.ReadToEnd();
			}
		}

	}
}
