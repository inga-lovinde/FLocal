using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using MySql.Data.MySqlClient;
using FLocal.Core.DB;

namespace FLocal.MySQLConnector {
	public class MySQLDBTraits : IDBTraits {

		public static readonly MySQLDBTraits instance = new MySQLDBTraits();

		public DbConnection createConnection(string connectionString) {
			return new MySqlConnection(connectionString);
		}

		public long LastInsertId(DbCommand command, ITableSpec table) {
			return ((MySqlCommand)command).LastInsertedId;
		}

		public string escapeIdentifier(string identifier) {
			return "`" + MySqlHelper.EscapeString(identifier) + "`";
		}

		public string markParam(string param) {
			return "@" + param;
		}

		public bool supportsIsolationLevel() {
			//for some reason, call to BeginTransaction with IsolationLevel set fails somewhere deep in mysql library
			return false;
		}

	}
}
