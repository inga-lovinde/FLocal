using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core.DB;
using MySql.Data.MySqlClient;

namespace FLocal.MySQLConnector {
	static class Extensions {

		public static string compile(this ITableSpec table) {
			return "`" + MySqlHelper.EscapeString(table.name) + "`";
		}

		public static string compile(this ColumnSpec column) {
			return column.table.compile() + ".`" + MySqlHelper.EscapeString(column.name) + "`";
		}
	}
}
