﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using Web.Core;
using System.IO;

namespace FLocal.Common {

	public class Config : Config<Config> {

		public readonly string AppInfo;

		public readonly string InitTime;

		public readonly Web.Core.DB.IDBConnection mainConnection;

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
		
		public readonly bool forceHttps;

		public readonly int MinPostId;

		public readonly int SessionLifetime;

		public readonly string DefaultModernSkin;
		public readonly string DefaultLegacySkin;
		public readonly string DefaultMachichara;

		public readonly ILogger Logger;

		protected Config(NameValueCollection data) : base() {
			this.InitTime = DateTime.Now.ToLongTimeString();
			this.AppInfo = data["AppInfo"];
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
			this.forceHttps = parseBool(data["ForceHTTPS"]);
			this.MinPostId = int.Parse(data["MinPostId"]);
			this.SessionLifetime = int.Parse(data["SessionLifetime"]);
			this.DefaultLegacySkin = data["DefaultLegacySkin"];
			this.DefaultModernSkin = data["DefaultModernSkin"];
			this.DefaultMachichara = data["DefaultMachichara"];
			this.Logger = new SingleFileLogger(this);
			this.mainConnection = new MySQLConnector.Connection(data["ConnectionString"], MySQLConnector.PostgresDBTraits.instance, this.Logger);
		}

		public static void Init(NameValueCollection data) {
			doInit(() => new Config(data));
		}

		/*public static void ReInit(NameValueCollection data) {
			doReInit(() => new Config(data));
		}*/

		public override void Dispose() {
			this.mainConnection.Dispose();
			base.Dispose();
		}

		public static void Transactional(Action<Web.Core.DB.Transaction> action) {
			using(Web.Core.DB.Transaction transaction = Web.Core.DB.IDBConnectionExtensions.beginTransaction(instance.mainConnection)) {
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

		private class SingleFileLogger : ILogger {

			private readonly StreamWriter writer;

			private readonly object locker = new object();

			public SingleFileLogger(Config config) {
				this.writer = new StreamWriter(config.dataDir + "Logs\\" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".mainlog.txt");
			}

			void ILogger.Log(string message) {
				lock(this.locker) {
					this.writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff") + ": " + message);
					this.writer.Flush();
				}
			}

		}

	}

}
