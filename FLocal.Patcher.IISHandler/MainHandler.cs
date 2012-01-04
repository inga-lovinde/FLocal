using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using FLocal.Patcher.Common;

namespace FLocal.Patcher.IISHandler {
	public class MainHandler : Patcher.Web.MainHandler {

		protected override Patcher.Web.PatcherInfo patcherInfo {
			get {
				return PatcherInfo.instance;
			}
		}

		protected override string GetAdminConnectionString(HttpContext context) {
			string[] parts = context.Request.Form["data"].Split(';');
			return System.Configuration.ConfigurationManager.AppSettings["Patcher.AdminConnectionString"].Replace("{username}", parts[0]).Replace("{password}", parts[1]);
		}

	}
}
