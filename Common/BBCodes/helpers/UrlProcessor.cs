using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using FLocal.Core;

namespace FLocal.Common.BBCodes {
	class UrlProcessor {

		public struct UrlInfo {
			public readonly bool isLocal;
			private readonly string rawRelativeUrl;
			public string relativeUrl {
				get {
					return this.rawRelativeUrl;
					//return HttpUtility.UrlEncode(this.rawRelativeUrl);
				}
			}
			public UrlInfo(bool isLocal, string relativeUrl) {
				this.isLocal = isLocal;
				this.rawRelativeUrl = relativeUrl;
			}
		}

		private static string Safe(string str) {
			return System.Web.HttpUtility.HtmlEncode(str);
		}

		public static UrlInfo Process(string url) {
			if (url.StartsWith("/")) {
				return new UrlInfo(true, url);
			}
			Uri uri;
			try {
				uri = new Uri(url);
			} catch(UriFormatException) {
				throw new Core.FLocalException("wrong url: " + url);
			}
			if (Config.instance.AdditionalHosts.Contains(uri.Host.ToLower()) || uri.Host.ToLower().EndsWith(Config.instance.BaseHost)) {
				return new UrlInfo(true, uri.PathAndQuery);
			} else {
				return new UrlInfo(false, uri.ToString());
			}
		}

		public static string ProcessLink(string link, string title, bool shortenRelative) {
			bool isExternal = true;
			string url;
			if(shortenRelative) {
				var urlInfo = UrlProcessor.Process(link);
				url = urlInfo.relativeUrl;
				isExternal = !urlInfo.isLocal;
			} else {
				var urlInfo = new Uri(link);
				url = urlInfo.ToString();
			}
			if(isExternal) {
				if(title == null) {
					title = Safe(url);
				}
			} else {
				var linkInfo = URL.UrlManager.Parse(url, new System.Collections.Specialized.NameValueCollection(), true);
				if(linkInfo == null) {
					throw new FLocalException("Unable to parse link: " + url);
				}
				url = linkInfo.canonicalFull;
				if(title == null) {
					title = Safe(linkInfo.title);
				}
			}
			string result = String.Format("<a href=\"{0}\"{1}>{2}</a>", url, (isExternal ? " class=\"external\"" : ""), title);
			return result;
		}

	}
}
