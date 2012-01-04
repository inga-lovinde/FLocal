using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Patcher.Data.Patch;

namespace Patcher.Web {
	class CheckParams : ICheckParams {

		protected readonly IPatcherConfiguration configuration;

		public CheckParams(IPatcherConfiguration configuration) {
			this.configuration = configuration;
		}

		public string DbDriverName {
			get {
				return this.configuration.DbDriverName;
			}
		}

		public virtual string ConnectionString {
			get {
				return this.configuration.GuestConnectionString;
			}
		}

		public string PatchesTable {
			get {
				return this.configuration.PatchesTable;
			}
		}

		public IEnumerable<PatchId> getPatchesList() {
			return this.configuration.getPatchesList();
		}

	}
}
