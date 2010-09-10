using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Web;

namespace FLocal.TexCompiler {
	public static class Compiler {

		private static readonly Encoding ENCODING = Encoding.UTF8;

		/// <summary>
		/// http://msdn.microsoft.com/en-us/library/system.io.stream.write.aspx
		/// </summary>
		private static void CopyTo(this Stream input, Stream output) {
			const int size = 4096;
			byte[] bytes = new byte[4096];
			int numBytes;
			while((numBytes = input.Read(bytes, 0, size)) > 0) {
				output.Write(bytes, 0, numBytes);
			}
		}

		public static Stream GetPngStream(string tex) {

			Dictionary<string, string> postData = new Dictionary<string,string> {
				{ "dev", "png16m" },
				{ "template", "textutf8" },
				{ "src", tex },
			};
			string post = string.Join("&", (from kvp in postData select string.Format("{0}={1}", HttpUtility.UrlEncode(kvp.Key, ENCODING), HttpUtility.UrlEncode(kvp.Value, ENCODING))).ToArray());
			byte[] postBytes = Encoding.ASCII.GetBytes(post);

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://sciencesoft.at/image/latexurl/img.png");
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
			request.ContentLength = postBytes.Length;
			using(Stream stream = request.GetRequestStream()) {
				stream.Write(postBytes, 0, postBytes.Length);
			}

			using(HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
				try {
					MemoryStream memoryStream = new MemoryStream();
					using(Stream responseStream = response.GetResponseStream()) {
						responseStream.CopyTo(memoryStream);
						memoryStream.Seek(0, SeekOrigin.Begin);
						return memoryStream;
					}
				} finally {
					response.Close();
				}
			}
		}

	}
}
