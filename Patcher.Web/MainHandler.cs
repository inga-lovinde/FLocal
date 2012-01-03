using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Patcher.Web {
	abstract public class MainHandler : IHttpHandler {

		abstract protected PatcherInfo patcherInfo {
			get;
		}

		abstract protected string GetAdminConnectionString(HttpContext context);

		public bool IsReusable {
			get { return true; }
		}

		private void Install(HttpContext context) {
			var writer = context.Response.Output;
			writer.WriteLine("<p>Installing...</p>");
			System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
			this.patcherInfo.AreNewPatchesInstalled = true;
			writer.WriteLine("<p>Installed</p>");
		}

		private void ShowInfo(HttpContext context) {
			var writer = context.Response.Output;
			writer.WriteLine("<form method=\"POST\"><input type=\"hidden\" name=\"install\" value=\"install\"/><input type=\"submit\" value=\"Install!\"/></form>");
		}

		public void ProcessRequest(HttpContext context) {
			if(context.Request.Form["install"] == "install") {
				this.Install(context);
			} else {
				this.ShowInfo(context);
			}
		}

	}
}
