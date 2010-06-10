using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace FLocal.MySQLConnector {
	public interface IDBProvider {

		DbConnection createConnection(string connectionString);

		long LastInsertId(DbCommand command);

	}
}
