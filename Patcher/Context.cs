using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Patcher.Data.Patch;
using Patcher.DB;
using System.IO;

namespace Patcher
{
	class Context
	{

		public readonly IConfig config;
		
		public readonly Func<List<PatchId>> getPatchesList;

		public readonly Func<PatchId, Stream> loadPatch;

		public readonly IDBTraits DbDriver;

		private static readonly Dictionary<string, IDBTraits> DB_DRIVERS = new Dictionary<string, IDBTraits>
		                                                              {
		                                                              	{ "oracle", OracleDBTraits.instance },
		                                                              	{ "oracle-faketransactional", OracleFakeTransactionalDBTraits.instance },
		                                                              };

		public Context(IConfig config, Func<IEnumerable<PatchId>> getPatchesListUnsorted, Func<PatchId, Stream> loadPatch) {
			this.config = config;
			this.getPatchesList = () => (from patchId in getPatchesListUnsorted() orderby patchId ascending select patchId).ToList();
			this.loadPatch = loadPatch;
			this.DbDriver = DB_DRIVERS[config.DbDriverName];
		}

	}
}
