using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using FLocal.Core;

namespace FLocal.IISHandler.handlers {
	class StaticHandler : AbstractGetHandler<FLocal.Common.URL.Static> {

		protected override string templateName {
			get {
				return null;
			}
		}

		protected override IEnumerable<System.Xml.Linq.XElement> getSpecificData(WebContext context) {

			string[] requestParts = this.url.remainder.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

			Regex checker = new Regex("^[a-z][0-9a-z\\-_]*(\\.[a-zA-Z]+)?$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);
			string path = "";
			for(int i=0; i<requestParts.Length; i++) {
				if(!checker.IsMatch(requestParts[i])) {
					//throw new HttpException(400, "wrong url (checker='" + checker.ToString() + "'; string='" + this.requestParts[i] + "'");
					throw new WrongUrlException();
				}
				path += FLocal.Common.Config.instance.DirSeparator + requestParts[i];
			}

			string fullPath = FLocal.Common.Config.instance.dataDir + "Static" + path;
			if(!File.Exists(fullPath)) {
				//throw new HttpException(404, "not found");
				throw new WrongUrlException();
			}
			FileInfo fileinfo = new FileInfo(fullPath);
			if(!fileinfo.FullName.StartsWith(FLocal.Common.Config.instance.dataDir + "Static")) {
				//throw new HttpException(403, "forbidden");
				throw new WrongUrlException();
			}
			
			string mime = Util.getMimeByExtension(fileinfo.Extension);
			if(mime != null) {
				context.httpresponse.ContentType = mime;
			} else {
				//throw new HttpException(403, "wrong file type");
				throw new WrongUrlException();
			}

			context.httpresponse.Cache.SetExpires(DateTime.Now.AddDays(10));
			context.httpresponse.Cache.SetLastModified(fileinfo.LastWriteTime);
			context.httpresponse.Cache.SetCacheability(HttpCacheability.Public);

			context.httpresponse.TransmitFile(fileinfo.FullName);

			throw new response.SkipXsltTransformException();
		}

	}
}
