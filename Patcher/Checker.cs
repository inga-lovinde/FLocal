using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Core;
using Patcher.Data.Patch;
using Patcher.DB;

namespace Patcher {
	public class Checker {

		private const string STATUS_INSTALLING = "installing";
		private const string STATUS_INSTALLED = "installed";

		private readonly ICheckParams checkParams;

		public Checker(ICheckParams checkParams) {
			this.checkParams = checkParams;
		}

		public IEnumerable<PatchId> GetPatchesToInstall() {
			List<PatchId> inputPatches = this.checkParams.getPatchesList().OrderBy(patchId => patchId).ToList();
			using(Transaction transaction = TransactionFactory.Create(this.checkParams.DbDriverName, this.checkParams.ConnectionString)) {
				return Util.RemoveExtra(
					from patchId in this.checkParams.getPatchesList()
					orderby patchId ascending
					select patchId,
					this.GetInstalledPatches()
				);
			}
		}

		public IEnumerable<PatchId> GetInstalledPatches() {
			using(Transaction transaction = TransactionFactory.Create(this.checkParams.DbDriverName, this.checkParams.ConnectionString)) {
				return (
					from row in transaction.ExecuteReader(
						string.Format(
							"select {1}, {2} from {0} where {3} = {4}",
							transaction.EscapeName(this.checkParams.PatchesTable),
							transaction.EscapeName("VERSION"),
							transaction.EscapeName("NAME"),
							transaction.EscapeName("STATUS"),
							transaction.MarkParam("pstatus")
						),
						new Dictionary<string, object> {
							{ "pstatus", STATUS_INSTALLED },
						}
					)
					let patch = new PatchId(int.Parse(row["VERSION"]), row["NAME"])
					orderby patch ascending
					select patch
				).ToList();
			}
		}

		public bool IsNeedsPatching() {
			return this.GetPatchesToInstall().Any();
		}

	}
}
