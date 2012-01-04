using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

namespace Patcher.Web {
	abstract public class MainHandler : IHttpHandler {

		private const string ACTION_INSTALLALL = "InstallAll";
		private const string ACTION_ROLLBACKLATEST = "RollbackLatest";

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
			var updater = new Updater(new UpdateParams(this.patcherInfo.configuration, this.GetAdminConnectionString(context)), new InteractiveResponseStream(context));
			int resultCode = updater.ApplyAll();
			if(resultCode == 0) {
				this.patcherInfo.PatchesInstalled();
				context.Response.Output.WriteLine("Installed");
			} else {
				context.Response.Output.WriteLine("Failed to install: error code {0}", resultCode);
			}
		}

		private void RollbackLatest(HttpContext context) {
			context.Response.ContentType = "text/plain";
			context.Response.Output.WriteLine("Uninstalling...");
			var updater = new Updater(new UpdateParams(this.patcherInfo.configuration, this.GetAdminConnectionString(context)), new InteractiveResponseStream(context));
			this.patcherInfo.DisallowMainHandler();
			int resultCode = updater.RollbackLastPatch();
			if(resultCode == 0) {
				context.Response.Output.WriteLine("Uninstalled");
			} else {
				context.Response.Output.WriteLine("Failed to uninstall: error code {0}", resultCode);
			}
		}

		private void WriteForm(TextWriter writer, string method, string methodDescription) {
			writer.WriteLine("<form method=\"POST\">");
			writer.WriteLine("<input type=\"password\" name=\"data\"/><br/>");
			writer.WriteLine("<input type=\"hidden\" name=\"{0}\" value=\"{0}\"/>", method);
			writer.WriteLine("<input type=\"submit\" value=\"{0}\"/>", methodDescription);
			writer.WriteLine("</form>");
		}

		private void ShowInfo(HttpContext context) {
			var writer = context.Response.Output;
			var checker = new Checker(new CheckParams(this.patcherInfo.configuration));

			{
				writer.WriteLine("<h1>Patches to install</h1>");
				int totalPatches = 0;

				writer.WriteLine("<ol>");
				foreach(var patchId in checker.GetPatchesToInstall()) {
					writer.WriteLine("<li>{0}: \"{1}\"</li>", patchId.version, patchId.name);
					totalPatches++;
				}
				writer.WriteLine("</ol>");

				writer.WriteLine("<p>Total patches: {0}</p>", totalPatches);
				if(totalPatches > 0) {
					WriteForm(writer, ACTION_INSTALLALL, "Install all patches");
				}
			}

			{
				writer.WriteLine("<h1>Installed patches</h1>");
				int totalPatches = 0;

				writer.WriteLine("<ol>");
				foreach(var patchId in checker.GetInstalledPatches()) {
					writer.WriteLine("<li>{0}: \"{1}\"</li>", patchId.version, patchId.name);
					totalPatches++;
				}
				writer.WriteLine("</ol>");

				writer.WriteLine("<p>Total patches: {0}</p>", totalPatches);
				if(totalPatches > 0) {
					WriteForm(writer, ACTION_ROLLBACKLATEST, "Uninstall latest patch");
				}
			}
		}

		public void ProcessRequest(HttpContext context) {
			if(context.Request.Form[ACTION_INSTALLALL] == ACTION_INSTALLALL) {
				this.Install(context);
			} else if(context.Request.Form[ACTION_ROLLBACKLATEST] == ACTION_ROLLBACKLATEST) {
				this.RollbackLatest(context);
			} else {
				this.ShowInfo(context);
			}
		}

	}
}
