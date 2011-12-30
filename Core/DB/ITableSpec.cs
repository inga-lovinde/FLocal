using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Core.DB {
	public interface ITableSpec {
		string name {
			get;
		}

		string idName {
			get;
		}

	}

	public static class ITableSpecExtensions {

		public static ColumnSpec getIdSpec(this ITableSpec table) {
			return new ColumnSpec(table, table.idName);
		}

		public static ColumnSpec getColumnSpec(this ITableSpec table, string column) {
			return new ColumnSpec(table, column);
		}

	}

}
