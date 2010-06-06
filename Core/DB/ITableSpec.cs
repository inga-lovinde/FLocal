using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core.DB {
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

	}

}
