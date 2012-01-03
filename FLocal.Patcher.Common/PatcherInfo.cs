using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Patcher.Common {
	public class PatcherInfo : Patcher.Web.PatcherInfo {

		public static readonly PatcherInfo instance = new PatcherInfo();

		private PatcherInfo() : base(PatcherConfiguration.instance) {
		}

	}
}
