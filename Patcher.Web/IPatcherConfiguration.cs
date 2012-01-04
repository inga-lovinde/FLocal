using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Patcher.Data.Patch;
using System.IO;

namespace Patcher.Web {
	public interface IPatcherConfiguration {

		string DbDriverName {
			get;
		}

		string GuestConnectionString {
			get;
		}

		string PatchesTable {
			get;
		}

		string EnvironmentName {
			get;
		}

		IEnumerable<PatchId> getPatchesList();

		Stream loadPatch(PatchId patchId);

		string LogDir {
			get;
		}

	}
}
