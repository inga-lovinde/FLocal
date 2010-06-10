using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace FLocal.MySQLConnector {
	public class MySQLProvider : IDBProvider {

		public static readonly MySQLProvider instance = new MySQLProvider();

		public DbConnection createConnection(string connectionString) {
			return new MySqlConnection(connectionString);
		}

		public long LastInsertId(DbCommand command) {
			return ((MySqlCommand)command).LastInsertedId;
		}

	}
}
