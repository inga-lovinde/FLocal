using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FLocal.IISHandler {
	class WebContext {

		public readonly HttpContext httpcontext;

		public HttpRequest httprequest {
			get {
				return this.httpcontext.Request;
			}
		}

		public HttpResponse httpresponse {
			get {
				return this.httpcontext.Response;
			}
		}

		public designs.IDesign design {
			get {
				return new designs.Classic();
			}
		}

		public Common.Config config {
			get {
				return Common.Config.instance;
			}
		}

		public WebContext(HttpContext httpcontext) {
			this.httpcontext = httpcontext;
		}

		public string Transform(string templateName, System.Xml.Linq.XDocument data) {
			//TODO: this should work according to design!
			return TemplateEngine.Compile(this.design.fsname + this.config.DirSeparator + templateName, data);
		}

	}
}
