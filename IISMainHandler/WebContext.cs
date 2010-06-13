﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using FLocal.Core;

namespace FLocal.IISHandler {
	class WebContext : Common.UserContext {

		public readonly HttpContext httpcontext;

		public HttpRequest httprequest {
			get {
				return this.httpcontext.Request;
			}
		}

		private object requestParts_Locker = new object();
		public string[] requestParts {
			get {
				return Cache<string[]>.instance.get(requestParts_Locker, () => this.httprequest.Path.Split("/", StringSplitOptions.RemoveEmptyEntries));
			}
		}

		public HttpResponse httpresponse {
			get {
				return this.httpcontext.Response;
			}
		}

		public override Common.dataobjects.IUserSettings userSettings {
			get {
				return new Common.dataobjects.AnonymousUserSettings();
			}
		}

		public override Common.IOutputParams outputParams {
			get {
				return this.design;
			}
		}

		public designs.IDesign design {
			get {
				return new designs.Classic();
			}
		}

		public override string formatDateTime(DateTime dateTime) {
			return dateTime.ToString();
		}

		public override System.Xml.Linq.XElement formatTotalPosts(long posts) {
			return PageOuter.create(this.userSettings.postsPerPage, posts).exportToXml(2, 0, 2);
		}

		public DateTime requestTime;

		public WebContext(HttpContext httpcontext) {
			this.httpcontext = httpcontext;
			this.requestTime = DateTime.Now;
		}

		public string Transform(string templateName, System.Xml.Linq.XDocument data) {
			//TODO: this should work according to design!
			return TemplateEngine.Compile(this.design.fsname + this.config.DirSeparator + templateName, data);
		}

	}
}
