using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using FLocal.Core;

namespace FLocal.Common {

	public class Config : Config<Config> {

		public readonly string InitTime;

		public readonly Core.DB.IDBConnection mainConnection;

		public readonly string dataDir;

		public readonly string DirSeparator;

		public readonly string SaltMigration;
		
		public readonly string SaltPasswords;

		public readonly string SaltUploader;

		public readonly string UploaderUrl;

		public readonly TimeSpan ActivityThreshold;

		public readonly string AdminUserName;

		public readonly bool IsIndexingDisabled;

		public readonly bool IsMigrationEnabled;

		public readonly string BaseHost;
		
		public readonly HashSet<string> AdditionalHosts;

		public readonly string CookiesPrefix;

		public readonly int MinPostId;

		public readonly int SessionLifetime;

		protected Config(NameValueCollection data) : base(data) {
			this.InitTime = DateTime.Now.ToLongTimeString();
			this.mainConnection = new MySQLConnector.Connection(data["ConnectionString"], MySQLConnector.PostgresDBTraits.instance);
			this.dataDir = data["DataDir"];
			this.DirSeparator = System.IO.Path.DirectorySeparatorChar.ToString();
			this.SaltMigration = data["SaltMigration"];
			this.SaltPasswords = data["SaltPasswords"];
			this.SaltUploader = data["SaltUploader"];
			this.UploaderUrl = data["UploaderUrl"];
			this.AdminUserName = data["AdminUserName"];
			this.ActivityThreshold = TimeSpan.FromMinutes(int.Parse(data["ActivityThreshold"]));
			this.IsIndexingDisabled = parseBool(data["DisableIndexing"]);
			this.IsMigrationEnabled = parseBool(data["EnableMigration"]);
			this.BaseHost = data["BaseHost"];
			this.AdditionalHosts = new HashSet<string>(from host in data["AdditionalHosts"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries) select host.Trim());
			this.CookiesPrefix = data["CookiesPrefix"];
			this.MinPostId = int.Parse(data["MinPostId"]);
			this.SessionLifetime = int.Parse(data["SessionLifetime"]);
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

		public static void Transactional(Action<Core.DB.Transaction> action) {
			using(Core.DB.Transaction transaction = Core.DB.IDBConnectionExtensions.beginTransaction(instance.mainConnection)) {
				bool success = false;
				try {
					action(transaction);
					success = true;
					transaction.Commit();
				} catch(FLocalException) {
					if(!success) {
						transaction.Rollback();
					}
					throw;
				}
			}
		}

	}

}
