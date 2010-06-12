using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.Win32;

namespace FLocal.IISHandler.handlers {
	class StaticHandler : ISpecificHandler {

		private string[] requestParts;

		public StaticHandler(string[] requestParts) {
			this.requestParts = requestParts;
		}

		public void Handle(WebContext context) {
			if(this.requestParts.Length < 2) {
				throw new HttpException(403, "listing not allowed");
			}
			
			Regex checker = new Regex("^[a-z][0-9a-z\\-]*(\\.[a-zA-Z]+)?$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);
			string path = "";
			for(int i=1; i<this.requestParts.Length; i++) {
				if(!checker.IsMatch(this.requestParts[i])) {
					throw new HttpException(400, "wrong url (checker='" + checker.ToString() + "'; string='" + this.requestParts[i] + "'");
				}
				path += FLocal.Common.Config.instance.DirSeparator + this.requestParts[i];
			}

			string fullPath = FLocal.Common.Config.instance.dataDir + "Static" + path;
			if(!File.Exists(fullPath)) {
				throw new HttpException(404, "not found");
			}
			FileInfo fileinfo = new FileInfo(fullPath);
			if(!fileinfo.FullName.StartsWith(FLocal.Common.Config.instance.dataDir + "Static")) {
				throw new HttpException(403, "forbidden");
			}
			
			RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(fileinfo.Extension);
			if (regKey != null && regKey.GetValue("Content Type") != null) {
				context.httpresponse.ContentType = regKey.GetValue("Content Type").ToString();
			} else {
				throw new HttpException(403, "wrong file type");
			}

			context.httpresponse.CacheControl = HttpCacheability.Public.ToString();
			context.httpresponse.Expires = 1440;

			context.httpresponse.WriteFile(fileinfo.FullName);
		}

	}
}
