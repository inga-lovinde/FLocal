﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

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

		public static HashSet<string> KnownAliases = new HashSet<string> {
			"forum.local",
			"forum.b.gz.ru",
			"www.snto-msu.net",
			"hq.sectorb.msk.ru",
			"petaflop.b.gz.ru",
			"194.88.210.5",
			"forumlocal.ru",
			"forumbgz.ru",
			"forum.hn",
		};

		public const string DOMAIN = ".forum.hn";

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
			if (KnownAliases.Contains(uri.Host.ToLower()) || uri.Host.ToLower().EndsWith(DOMAIN)) {
				return new UrlInfo(true, uri.PathAndQuery);
			} else {
				return new UrlInfo(false, uri.ToString());
			}
		}

	}
}
