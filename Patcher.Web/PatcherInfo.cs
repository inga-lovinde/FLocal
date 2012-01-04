using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Web {
	abstract public class PatcherInfo {

		internal readonly IPatcherConfiguration configuration;

		internal readonly bool IsContainsNewPatches;

		internal bool AreNewPatchesInstalled {
			get;
			private set;
		}

		private bool IsMainHandlerDisallowed;

		public bool IsNeedsPatching {
			get {
				return (this.IsContainsNewPatches && !this.AreNewPatchesInstalled) || this.IsMainHandlerDisallowed;
			}
		}

		internal void PatchesInstalled() {
			this.AreNewPatchesInstalled = true;
		}

		internal void DisallowMainHandler() {
			this.IsMainHandlerDisallowed = true;
		}

		protected PatcherInfo(IPatcherConfiguration configuration) {
			this.configuration = configuration;
			this.IsContainsNewPatches = (new Checker(new CheckParams(configuration))).IsNeedsPatching();
			this.IsMainHandlerDisallowed = false;
		}

	}
}
