using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Core;
using Patcher.Data.Patch;
using Patcher.DB;
using System.IO;

namespace Patcher
{
	class Context
	{

		private readonly IUpdateParams updateParams;
		
		public List<PatchId> getPatchesList() {
			return (from patchId in this.updateParams.getPatchesList() orderby patchId ascending select patchId).ToList();
		}

		public readonly IDBTraits DbDriver;

		public readonly IInteractiveConsole console;

		public string EnvironmentName {
			get {
				return this.updateParams.EnvironmentName;
			}
		}

		public string PatchesTable {
			get {
				return this.updateParams.PatchesTable;
			}
		}

		public Stream loadPatch(PatchId patchId) {
			return this.updateParams.loadPatch(patchId);
		}

		public Transaction CreateTransaction() {
			return TransactionFactory.Create(this.updateParams.DbDriverName, this.updateParams.ConnectionString);
		}

		public Context(IUpdateParams updateParams, IInteractiveConsole console) {
			this.updateParams = updateParams;
			this.DbDriver = DBTraitsFactory.GetTraits(updateParams.DbDriverName);
			this.console = console;
		}

	}
}
