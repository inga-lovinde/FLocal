using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Patcher.Data.Patch;

namespace Patcher.Web {
	class UpdateParams : CheckParams, IUpdateParams {

		private readonly string AdminConnectionString;

		public UpdateParams(IPatcherConfiguration configuration, string AdminConnectionString) : base(configuration) {
			this.AdminConnectionString = AdminConnectionString;
		}

		public override string ConnectionString {
			get {
				return this.AdminConnectionString;
			}
		}

		public string EnvironmentName {
			get {
				return configuration.EnvironmentName;
			}
		}

		public Stream loadPatch(PatchId patchId) {
			return configuration.loadPatch(patchId);
		}

	}
}
