using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace FLocal.Common {

	public class Config : FLocal.Core.Config<Config> {

		public readonly string InitTime;

		public readonly Core.DB.IDBConnection mainConnection;

		public readonly string dataDir;

		public readonly string DirSeparator;

		public readonly string SaltMigration;
		
		public readonly string SaltPasswords;

		protected Config(NameValueCollection data) : base(data) {
			this.InitTime = DateTime.Now.ToLongTimeString();
			this.mainConnection = new MySQLConnector.Connection(data["ConnectionString"], MySQLConnector.PostgresDBTraits.instance);
			this.dataDir = data["DataDir"];
			this.DirSeparator = System.IO.Path.DirectorySeparatorChar.ToString();
			this.SaltMigration = data["SaltMigration"];
			this.SaltPasswords = data["SaltPasswords"];
		}

		public static void Init(NameValueCollection data) {
			doInit(() => new Config(data));
		}

		public static void ReInit(NameValueCollection data) {
			doReInit(() => new Config(data));
		}

		public override void Dispose() {
			this.mainConnection.Dispose();
			base.Dispose();
		}

	}

}
