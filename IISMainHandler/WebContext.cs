using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Web.Core;
using FLocal.Common;
using FLocal.Common.dataobjects;
using FLocal.Common.actions;
using System.Xml.Linq;
using System.IO;

namespace FLocal.IISHandler {
	class WebContext : FLocal.Common.UserContext {

		private static readonly Encoding OutputEncoding = Encoding.UTF8;

		public readonly HttpContext httpcontext;

		public HttpRequest httprequest {
			get {
				return this.httpcontext.Request;
			}
		}

		/*private object requestParts_Locker = new object();
		private string[] requestParts_Data = null;
		public string[] requestParts {
			get {
				if(this.requestParts_Data == null) {
					lock(this.requestParts_Locker) {
						if(this.requestParts_Data == null) {
							this.requestParts_Data = this.httprequest.Path.Split("/", StringSplitOptions.RemoveEmptyEntries);
						}
					}
				}
				return this.requestParts_Data;
			}
		}*/

		public XElement exportRequestParameters() {
			return new XElement("get",
				from i in Enumerable.Range(0, this.httprequest.QueryString.Count)
				where this.httprequest.QueryString.GetKey(i) != null
				select new XElement("param",
					new XAttribute("name", this.httprequest.QueryString.GetKey(i)),
					this.httprequest.QueryString[i]
				)
			);
		}

		public HttpResponse httpresponse {
			get {
				return this.httpcontext.Response;
			}
		}

		public IUserSettings userSettings {
			get;
			private set;
		}

		public override FLocal.Common.IOutputParams outputParams {
			get {
				return this.design;
			}
		}

		public readonly designs.IDesign design;

		public override string formatDateTime(DateTime dateTime) {
			return dateTime.ToString();
		}

		public override System.Xml.Linq.XElement formatTotalPosts(long posts) {
			return PageOuter.create(this.userSettings.postsPerPage, posts).exportToXml(2, 0, 2);
		}

		public DateTime requestTime {
			get;
			private set;
		}

		public Session session;

		public override Account account {
			get {
				if(this.session == null) {
					return null;
				}
				return this.session.account;
			}
		}

		public override PostVisibilityEnum isPostVisible(Post post) {
			return this.userSettings.isPostVisible(post);
		}

		private designs.IDesign detectDesign() {
			switch(this.httprequest.Url.Port % 1000) {
				case 445:
					return new designs.Raw();
				case 447:
					return new designs.Lite();
				case 449:
					return new designs.Rss();
				case 451:
					return new designs.Classic();
				case 443:
				default:
					string[] parts = this.httprequest.Url.Host.Split('.');
					switch(parts[0].ToLower()) {
						case "raw":
							return new designs.Raw();
						case "lite":
							return new designs.Lite();
						case "rss":
							return new designs.Rss();
						case "classic":
							return new designs.Classic();
						case "modern":
						default:
							return new designs.Modern();
					}
			}
		}

		public WebContext(HttpContext httpcontext) {
			this.httpcontext = httpcontext;
			this.requestTime = DateTime.Now;
			this.design = this.detectDesign();

			HttpCookie sessionCookie = this.httprequest.Cookies[Config.instance.CookiesPrefix + "_session"];
			if(sessionCookie != null && sessionCookie.Value != null && sessionCookie.Value != "") {
				try {
					var session = Session.LoadByKey(sessionCookie.Value);
					var tmp = session.account;
					string lastUrl = null;
					if(this.httprequest.RequestType == "GET") {
						if(this.design.IsHuman) {
							lastUrl = this.httprequest.Path;
						}
					}
					session.updateLastActivity(lastUrl);
					HttpCookie newCookie = this.createCookie(Config.instance.CookiesPrefix + "_session");
					newCookie.Value = session.sessionKey;
					newCookie.Expires = DateTime.Now.AddSeconds(Config.instance.SessionLifetime);
					this.httpresponse.AppendCookie(newCookie);
					this.session = session;
				} catch(NotFoundInDBException) {
					sessionCookie.Value = "";
					sessionCookie.Expires = DateTime.Now.AddDays(-1);
					this.httpresponse.AppendCookie(sessionCookie);
					//throw; //TODO: remove me!
				}
			}
			if(this.session != null) {
				this.userSettings = AccountSettings.LoadByAccount(this.session.account);
			} else {
				this.userSettings = new AnonymousUserSettings(null);
			}
		}

		public void WriteTransformResult(string templateName, System.Xml.Linq.XDocument data) {
			this.httpresponse.ContentType = this.design.ContentType;
			this.httpresponse.ContentEncoding = OutputEncoding;
			TemplateEngine.WriteCompiled(this.design.GetFSName(templateName), data, this.httpresponse.Output);
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

		private void AddCommonData(HttpCookie cookie) {
			cookie.HttpOnly = true;
			cookie.Secure = Config.instance.forceHttps;
			cookie.Domain = "." + String.Join(".", this.httprequest.Url.Host.Split(".", StringSplitOptions.RemoveEmptyEntries).Slice(1).ToArray());
			cookie.Path = "/";
		}

		public HttpCookie createCookie(string name) {
			HttpCookie result = new HttpCookie(name);
			this.AddCommonData(result);
			return result;
		}

		public Web.Core.Network.IPv4Address remoteHost {
			get {
				return new Web.Core.Network.IPv4Address(this.httprequest.UserHostAddress);
			}
		}

		public void LogError(Exception e) {
			string dir;
			if(e is AccessDeniedException) {
				dir = FLocal.Common.Config.instance.dataDir + "Logs\\AccessDenied\\";
			} else {
				dir = FLocal.Common.Config.instance.dataDir + "Logs\\";
			}
			using(StreamWriter writer = new StreamWriter(dir + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "." + e.GetGuid().ToString() + ".txt")) {
				writer.WriteLine("Requested url: " + this.httprequest.Url.ToString());
				foreach(string key in this.httprequest.Form.Keys) {
					writer.WriteLine(string.Format("Form[{0}]: {1}", key, this.httprequest.Form[key]));
				}
				writer.WriteLine("Remote ip: " + this.httprequest.UserHostAddress);
				if(this.httprequest.UrlReferrer != null) {
					writer.WriteLine("Referer: " + this.httprequest.UrlReferrer.ToString());
				}
				if(this.httprequest.Cookies[Config.instance.CookiesPrefix + "_session"] != null) {
					writer.WriteLine("Session: " + this.httprequest.Cookies[Config.instance.CookiesPrefix + "_session"].Value);
				}
				writer.WriteLine();
				writer.WriteLine("Exception: " + e.GetType().FullName);
				writer.WriteLine("Guid: " + e.GetGuid().ToString());
				writer.WriteLine(e.Message);
				if(e is FLocalException) {
					writer.WriteLine(((FLocalException)e).FullStackTrace);
				} else {
					writer.WriteLine(e.StackTrace);
				}
			}
		}

	}
}
