using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.DB {
	static class DBTraitsFactory {

		public static IDBTraits GetTraits(string DbDriverName) {
			switch(DbDriverName.ToLower()) {
				case "oracle":
					return OracleDBTraits.instance;
				case "oracle-faketransactional":
					return OracleFakeTransactionalDBTraits.instance;
				case "postgres":
					return PostgresDBTraits.instance;
				default:
					throw new NotImplementedException();
			}
		}

	}
}
