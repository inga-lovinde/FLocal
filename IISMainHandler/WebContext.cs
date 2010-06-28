using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using FLocal.Core;
using FLocal.Common.dataobjects;
using FLocal.Common.actions;
using System.Xml.Linq;

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

		private IUserSettings _userSettings;
		public override IUserSettings userSettings {
			get {
				return this._userSettings;
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

		public Session session;

		public override Account account {
			get {
				if(this.session == null) {
					return null;
				}
				return this.session.account;
			}
		}

		public WebContext(HttpContext httpcontext) {
			this.httpcontext = httpcontext;
			this.requestTime = DateTime.Now;
			HttpCookie sessionCookie = this.httprequest.Cookies["session"];
			if(sessionCookie != null && sessionCookie.Value != null && sessionCookie.Value != "") {
				try {
					this.session = Session.LoadByKey(sessionCookie.Value);
					sessionCookie.Expires = DateTime.Now.AddDays(3);
					this.session.updateLastActivity();
					this.httpresponse.AppendCookie(sessionCookie);
				} catch(NotFoundInDBException) {
					sessionCookie.Value = "";
					sessionCookie.Expires = DateTime.Now.AddDays(-1);
					this.httpresponse.AppendCookie(sessionCookie);
					//throw; //TODO: remove me!
				}
			}
			if(this.session != null) {
				this._userSettings = AccountSettings.LoadByAccount(this.session.account);
			} else {
				this._userSettings = new AnonymousUserSettings();
			}
		}

		public string Transform(string templateName, System.Xml.Linq.XDocument data) {
			//TODO: this should work according to design!
			return TemplateEngine.Compile(this.design.fsname + this.config.DirSeparator + templateName, data);
		}

		public XElement exportSession() {
			if(this.session != null) {
				return session.exportToXml(this);
			} else {
				return new XElement("session",
					new XElement("notLoggedIn", true)
				);
			}
		}

		public HttpCookie createCookie(string name) {
			HttpCookie result = new HttpCookie(name);
			result.HttpOnly = true;
			result.Secure = true;
			result.Domain = "." + String.Join(".", this.httprequest.Url.Host.Split(".", StringSplitOptions.RemoveEmptyEntries).Slice(1).ToArray());
			result.Path = "/";
			return result;
		}

	}
}
