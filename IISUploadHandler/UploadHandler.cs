using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using Web.Core;

namespace Web.IISUploadHandler {
	class UploadHandler : IHttpHandler {

		public bool IsReusable {
			get {
				return true;
			}
		}

		public void ProcessRequest(HttpContext httpcontext) {
			if(httpcontext.Request.Path.ToLower() == "/upload/") {
				this.ProcessUpload(httpcontext);
			} else if(httpcontext.Request.Path.ToLower().StartsWith("/data/")) {
				this.ProcessRetrieve(httpcontext);
			} else {
				throw new HttpException(403, "wrong url");
			}
		}

		private static string getFilePath(string md5, string extension) {
			foreach(char chr in (md5 + extension)) {
				if(!Char.IsLetterOrDigit(chr)) throw new HttpException(403, "wrong md5 or extension");
			}
			return Config.instance.storageDir + md5.PHPSubstring(0, 2) + Path.DirectorySeparatorChar + md5.PHPSubstring(2, 2) + Path.DirectorySeparatorChar + md5.PHPSubstring(4) + "." + extension;
		}

		private static void CreateDirectoryIfNotExists(DirectoryInfo directoryInfo) {
			if(!directoryInfo.Exists) {
				CreateDirectoryIfNotExists(directoryInfo.Parent);
				directoryInfo.Create();
			}
		}

		private void ProcessUpload(HttpContext context) {
			byte[] data = new byte[context.Request.InputStream.Length];
			if(context.Request.InputStream.Read(data, 0, (int)context.Request.InputStream.Length) != context.Request.InputStream.Length) {
				throw new FLocalException("File is not uploaded correctly");
			}
			
			string file_md5 = Util.md5(context.Request.InputStream);

			string md5 = Util.md5(file_md5 + " " + Config.instance.salt);
			if(md5 != context.Request.QueryString["signature"]) {
				throw new HttpException(403, "signature mismatch");
			}

			string filePath = getFilePath(file_md5, context.Request.QueryString["extension"]);
			CreateDirectoryIfNotExists((new FileInfo(filePath)).Directory);
			using(FileStream stream = new FileStream(filePath, FileMode.CreateNew)) {
				stream.Write(data, 0, data.Length);
			}

			context.Response.Write("OK");
		}

		private void ProcessRetrieve(HttpContext context) {
			string[] requestParts = context.Request.Path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
			if(requestParts.Length != 2) throw new HttpException(403, "wrong url");
			
			string[] fileParts = requestParts[1].Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
			if(fileParts.Length != 2) throw new HttpException(403, "wrong url");
			
			string mime = Util.getMimeByExtension(fileParts[1]);
			if(mime == null) throw new FLocalException("unknown extension '" + fileParts[1] + "'");
			context.Response.ContentType = mime;
			
			context.Response.Cache.SetExpires(DateTime.Now.AddDays(10));
			context.Response.Cache.SetLastModified(DateTime.Now.AddYears(-1));
			context.Response.Cache.SetCacheability(HttpCacheability.Public);

			context.Response.TransmitFile(getFilePath(fileParts[0], fileParts[1]));
		}

	}
}
