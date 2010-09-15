using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using FLocal.Core;
using FLocal.Common;
using FLocal.Common.dataobjects;
using FLocal.Common.actions;
using System.Xml.Linq;
using System.IO;

namespace FLocal.IISHandler {
	class WebContext : Common.UserContext {

		private static readonly Encoding OutputEncoding = Encoding.UTF8;

		public readonly HttpContext httpcontext;

		public HttpRequest httprequest {
			get {
				return this.httpcontext.Request;
			}
		}

		private object requestParts_Locker = new object();
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
		}

		public XElement exportRequestParameters() {
			return new XElement("get",
				from i in Enumerable.Range(0, this.httprequest.QueryString.Count)
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

		public override Common.IOutputParams outputParams {
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

		public override bool isPostVisible(Post post) {
			return this.userSettings.isPostVisible(post);
		}

		public WebContext(HttpContext httpcontext) {
			this.httpcontext = httpcontext;
			this.requestTime = DateTime.Now;
			HttpCookie sessionCookie = this.httprequest.Cookies["session"];
			if(sessionCookie != null && sessionCookie.Value != null && sessionCookie.Value != "") {
				try {
					var session = Session.LoadByKey(sessionCookie.Value);
					var tmp = session.account;
					sessionCookie.Expires = DateTime.Now.AddDays(3);
					session.updateLastActivity(this.httprequest.RequestType == "GET" ? this.httprequest.Path : null);
					this.httpresponse.AppendCookie(sessionCookie);
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
				this.userSettings = new AnonymousUserSettings();
			}

			switch(this.httprequest.Url.Port % 1000) {
				case 445:
					this.design = new designs.Raw();
					break;
				case 447:
					this.design = new designs.Lite();
					break;
				case 449:
					this.design = new designs.Rss();
					break;
				case 443:
				default:
					this.design = new designs.Classic();
					break;
			}
		}

		public void WriteTransformResult(string templateName, System.Xml.Linq.XDocument data) {
			this.httpresponse.ContentType = this.design.ContentType;
			this.httpresponse.ContentEncoding = OutputEncoding;
			TemplateEngine.WriteCompiled(this.design.GetFSName(templateName), data, OutputEncoding, this.httpresponse.Output);
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

		public Core.Network.IPv4Address remoteHost {
			get {
				return new Core.Network.IPv4Address(this.httprequest.UserHostAddress);
			}
		}

		public void LogError(Exception e) {
			using(StreamWriter writer = new StreamWriter(Common.Config.instance.dataDir + "Logs\\" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "." + e.GetGuid().ToString() + ".txt")) {
				writer.WriteLine("Requested url: " + this.httprequest.Url.ToString());
				foreach(string key in this.httprequest.Form.Keys) {
					writer.WriteLine(string.Format("Form[{0}]: {1}", key, this.httprequest.Form[key]));
				}
				writer.WriteLine("Remote ip: " + this.httprequest.UserHostAddress);
				if(this.httprequest.UrlReferrer != null) {
					writer.WriteLine("Referer: " + this.httprequest.UrlReferrer.ToString());
				}
				if(this.httprequest.Cookies["session"] != null) {
					writer.WriteLine("Session: " + this.httprequest.Cookies["session"].Value);
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
