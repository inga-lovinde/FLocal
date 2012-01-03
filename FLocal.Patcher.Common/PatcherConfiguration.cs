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
			get { throw new NotImplementedException(); }
		}

		private readonly string _GuestConnectionString;
		public string GuestConnectionString {
			get { throw new NotImplementedException(); }
		}

		private readonly string _PatchesTable;
		public string PatchesTable {
			get { throw new NotImplementedException(); }
		}

		private readonly string _EnvironmentName;
		public string EnvironmentName {
			get { throw new NotImplementedException(); }
		}

		public IEnumerable<PatchId> getPatchesList() {
			return PatchesLoader.getPatchesList();
		}

		public Stream loadPatch(PatchId patchId) {
			return PatchesLoader.loadPatch(patchId);
		}

		protected PatcherConfiguration(NameValueCollection data) : base() {
			this._DbDriverName = data["Patcher.DbDriver"];
			this._EnvironmentName = data["Patcher.EnvironmentName"];
			this._GuestConnectionString = data["ConnectionString"];
			this._PatchesTable = data["Patcher.PatchesTable"];
		}

		public static void Init(NameValueCollection data) {
			doInit(() => new PatcherConfiguration(data));
		}

	}
}
