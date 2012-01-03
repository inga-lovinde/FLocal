using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Patcher.Data.Patch;

namespace Patcher {
	public interface IUpdateParams : ICheckParams {

		string EnvironmentName {
			get;
		}

		Stream loadPatch(PatchId patchId);

	}
}
