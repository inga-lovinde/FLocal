using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Npgsql;
using FLocal.Core.DB;

namespace FLocal.MySQLConnector {
	public class PostgresDBTraits : IDBTraits {

		public static readonly PostgresDBTraits instance = new PostgresDBTraits();

		public DbConnection createConnection(string connectionString) {
			return new Npgsql.NpgsqlConnection(connectionString);
		}

		public long LastInsertId(DbCommand command, ITableSpec table) {
			string sequenceName = table.name + "_" + table.idName + "_seq";
			using(DbCommand newCommand = command.Connection.CreateCommand()) {
				if(command.Transaction != null) {
					newCommand.Transaction = command.Transaction;
				}
				newCommand.CommandType = System.Data.CommandType.Text;
				newCommand.CommandText = "SELECT CURRVAL(" + this.escapeIdentifier(sequenceName) + ")";
				return (long)newCommand.ExecuteScalar();
			}
		}

		public string escapeIdentifier(string identifier) {
			return "\"" + identifier.Replace("\"", "") + "\"";
		}

		public string markParam(string param) {
			return ":" + param;
		}

		public bool supportsIsolationLevel() {
			return true;
		}

	}
}
