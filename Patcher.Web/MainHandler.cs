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
			context.Response.ContentType = "text/plain";
			context.Response.Output.WriteLine("Installing...");
			System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
			var updater = new Updater(new UpdateParams(this.patcherInfo.configuration, this.GetAdminConnectionString(context)), new InteractiveResponseStream(context));
			int resultCode = updater.ApplyAll();
			if(resultCode == 0) {
				this.patcherInfo.AreNewPatchesInstalled = true;
				context.Response.Output.WriteLine("Installed");
			} else {
				context.Response.Output.WriteLine("Failed to install: error code {0}", resultCode);
			}
		}

		private void ShowInfo(HttpContext context) {
			var writer = context.Response.Output;
			var checker = new Checker(new CheckParams(this.patcherInfo.configuration));
			int totalPatches = 0;
			foreach(var patchId in checker.GetPatchesToInstall()) {
				writer.WriteLine("<p>{0}: \"{1}\"</p>", patchId.version, patchId.name);
				totalPatches++;
			}
			writer.WriteLine("<p>Total patches: {0}", totalPatches);
			if(totalPatches > 0) {
				writer.WriteLine("<form method=\"POST\">");
				writer.WriteLine("<input type=\"text\" name=\"data\"/><br/>");
				writer.WriteLine("<input type=\"hidden\" name=\"install\" value=\"install\"/>");
				writer.WriteLine("<input type=\"submit\" value=\"Install!\"/>");
				writer.WriteLine("</form>");
			}
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
