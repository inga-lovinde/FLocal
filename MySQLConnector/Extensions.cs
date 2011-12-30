using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Core.DB;
using System.Data.Common;

namespace MySQLConnector {
	static class Extensions {

		public static string compile(this ITableSpec table, IDBTraits traits) {
			return traits.escapeIdentifier(table.name);
		}

		public static string compile(this ColumnSpec column, IDBTraits traits) {
			return column.table.compile(traits) + "." + traits.escapeIdentifier(column.name);
		}
	
		public static void AddParameter(this DbCommand command, string name, string value) {
			DbParameter parameter = command.CreateParameter();
			parameter.ParameterName = name;
			parameter.Value = value;
			command.Parameters.Add(parameter);
		}
	}
}
