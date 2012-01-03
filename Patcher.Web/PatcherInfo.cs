using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Web {
	abstract public class PatcherInfo {

		internal readonly IPatcherConfiguration configuration;

		public readonly bool IsContainsNewPatches;

		public bool AreNewPatchesInstalled {
			get;
			internal set;
		}

		public bool IsNeedsPatching {
			get {
				return this.IsContainsNewPatches && !this.AreNewPatchesInstalled;
			}
		}

		protected PatcherInfo(IPatcherConfiguration configuration) {
			this.configuration = configuration;
			this.IsContainsNewPatches = (new Checker(new CheckParams(configuration))).IsNeedsPatching();
		}

	}
}
