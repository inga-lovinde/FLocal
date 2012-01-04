using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.IO;
using Patcher.Data.Patch;
using Web.Core;
using Patcher.Web;

namespace FLocal.Patcher.Common {
	public class PatcherConfiguration : Config<PatcherConfiguration>, IPatcherConfiguration {

		private readonly string _DbDriverName;
		public string DbDriverName {
			get {
				return this._DbDriverName;
			}
		}

		private readonly string _GuestConnectionString;
		public string GuestConnectionString {
			get {
				return this._GuestConnectionString;
			}
		}

		private readonly string _PatchesTable;
		public string PatchesTable {
			get {
				return this._PatchesTable;
			}
		}

		private readonly string _EnvironmentName;
		public string EnvironmentName {
			get {
				return this._EnvironmentName;
			}
		}

		private readonly string _LogDir;
		public string LogDir {
			get {
				return this._LogDir;
			}
		}

		public IEnumerable<PatchId> getPatchesList() {
			return PatchesLoader.getPatchesList();
		}

		public Stream loadPatch(PatchId patchId) {
			return PatchesLoader.loadPatch(patchId);
		}

		protected PatcherConfiguration(NameValueCollection data) : base() {
			this._DbDriverName = data["Patcher.DbDriver"].ToString();
			this._EnvironmentName = data["Patcher.EnvironmentName"].ToString();
			this._GuestConnectionString = data["ConnectionString"].ToString();
			this._PatchesTable = data["Patcher.PatchesTable"].ToString();
			this._LogDir = Path.Combine(data["DataDir"], "Logs");
		}

		public static void Init(NameValueCollection data) {
			doInit(() => new PatcherConfiguration(data));
		}

	}
}
