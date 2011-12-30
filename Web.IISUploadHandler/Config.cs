using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Web.IISUploadHandler {
	class Config {

		public static readonly Config instance = new Config();

		public readonly string salt;
		public readonly string storageDir;

		private Config() {
			var appSettings = ConfigurationManager.AppSettings;
			this.salt = appSettings["salt"];
			this.storageDir = appSettings["storageDir"];
		}

	}
}
