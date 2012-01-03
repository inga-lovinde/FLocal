using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Patcher.Data.Patch;

namespace FLocal.Patcher.Common {
	static class PatchesLoader {

		private static readonly Regex PatchName = new Regex("^Patch_(?<version>[01-9]+)_(?<name>[a-z]+)\\.xml$", RegexOptions.Compiled | RegexOptions.ExplicitCapture);

		public static IEnumerable<PatchId> getPatchesList() {
			return
				from resourceName in Resources.ResourcesManager.GetResourcesList()
				where PatchName.IsMatch(resourceName)
				let match = PatchName.Match(resourceName)
				select new PatchId(int.Parse(match.Groups["version"].Value), match.Groups["name"].Value);
		}

		public static Stream loadPatch(PatchId patchId) {
			return Resources.ResourcesManager.GetResource(String.Format("Patch_{0:D5}_{1}.xml", patchId.version, patchId.name));
		}

		//public static 

	}
}
